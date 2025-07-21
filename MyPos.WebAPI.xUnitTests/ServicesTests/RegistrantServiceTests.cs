using AutoMapper;
using ExternalApi;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DTOs;
using WebAPI.Entities;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services;
using WebAPI.UnitOfWork;
using Xunit;
using Microsoft.EntityFrameworkCore;
using WebAPI.Exceptions;
using Microsoft.AspNetCore.Http;

namespace MyPos.WebAPI.XUnitTests.ServiceTests
{
    public class RegistrantServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IMapper> _mapperServiceMock;
        private readonly Mock<IRepository<RegistrantEntity>> _repoMock;
        private readonly RegistrantService _registrantService;

        public RegistrantServiceTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _mapperServiceMock = new Mock<IMapper>();
            _repoMock = new Mock<IRepository<RegistrantEntity>>();

            _registrantService = new RegistrantService(_uowMock.Object, _mapperServiceMock.Object);
        }

        [Fact]
        public async Task CreateRegistrant_ShouldAddRegistrantAndReturnDto()
        {
            var createDto = new CreateRegistrantDto
            {
                DisplayName = "Georgi Ivanov",
                GSM = "0897622444",
                Country = "Bulgaria",
                Address = "ul. Rosa 123",
                IsCompany = false,
            };
            var registrantEntity = new RegistrantEntity
            {
                DisplayName = createDto.DisplayName,
                GSM = createDto.GSM,
                Country = createDto.Country,
                Address = createDto.Address,
                isCompany = createDto.IsCompany
            };
            var expectedDto = new RegistrantDto
            {
                Id = 1,
                DisplayName = createDto.DisplayName,
                GSM = createDto.GSM,
                Country = createDto.Country,
                Address = createDto.Address,
                IsCompany = createDto.IsCompany,
                DateCreated = DateTime.Now
            };

            _mapperServiceMock
                .Setup(m => m.Map<RegistrantEntity>(It.IsAny<CreateRegistrantDto>()))
                .Returns(registrantEntity);


            _mapperServiceMock
                .Setup(m => m.Map<RegistrantDto>(It.IsAny<RegistrantEntity>()))
                .Returns((RegistrantEntity entity) => new RegistrantDto
                {
                    Id = 1,
                    DisplayName = entity.DisplayName,
                    GSM = entity.GSM,
                    Country = entity.Country,
                    Address = entity.Address,
                    IsCompany = entity.isCompany,
                    DateCreated = entity.DateCreated
                });

            _uowMock.Setup(u => u.GetRepository<RegistrantEntity>()).Returns(_repoMock.Object);
            _repoMock.Setup(r => r.AddAsync(It.IsAny<RegistrantEntity>())).Returns(Task.CompletedTask);
            _uowMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _registrantService.CreateRegistrant(createDto);

            Assert.NotNull(result);
            Assert.Equal(createDto.DisplayName, result.DisplayName);
            Assert.Equal(createDto.GSM, result.GSM);
            Assert.Equal(createDto.Country, result.Country);
            Assert.Equal(createDto.Address, result.Address);
            Assert.Equal(createDto.IsCompany, result.IsCompany);
            Assert.True(result.DateCreated <= DateTime.Now && result.DateCreated > DateTime.Now.AddMinutes(-1));

            _mapperServiceMock.Verify(m => m.Map<RegistrantEntity>(createDto), Times.Once());
            _repoMock.Verify(r => r.AddAsync(It.Is<RegistrantEntity>(r =>
                r.DisplayName == createDto.DisplayName &&
                r.GSM == createDto.GSM)), Times.Once);
            _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteRegistrant_ShouldDeleteAndReturnDto_WhenExists()
        {
            var id = 1;
            var entity = new RegistrantEntity { Id = id, DisplayName = "Test User" };
            var dto = new RegistrantDto { Id = id, DisplayName = "Test User" };

            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
            _mapperServiceMock.Setup(m => m.Map<RegistrantDto>(entity)).Returns(dto);
            _uowMock.Setup(u => u.GetRepository<RegistrantEntity>()).Returns(_repoMock.Object);
            _uowMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _registrantService.DeleteRegistrant(id);

            Assert.Equal(id, result.Id);
            _repoMock.Verify(r => r.Delete(entity), Times.Once);
            _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteRegistrant_ShouldThrow_WhenNotFound()
        {
            var id = 999;
            _repoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((RegistrantEntity)null);
            _uowMock.Setup(u => u.GetRepository<RegistrantEntity>()).Returns(_repoMock.Object);

            var ex = await Assert.ThrowsAsync<MyPosApiException>(() => _registrantService.DeleteRegistrant(id));

            Assert.Equal(StatusCodes.Status404NotFound, ex.StatusCode);
        }

        [Fact]
        public async Task GetById_ShouldReturnDto_WhenExists()
        {
            var id = 1;
            var entity = new RegistrantEntity { Id = id, DisplayName = "Test" };
            var dto = new RegistrantDto { Id = id, DisplayName = "Test" };

            _repoMock.Setup(r => r
                .GetSingleAsync(It.IsAny<Func<IQueryable<RegistrantEntity>, IQueryable<RegistrantEntity>>>()))
                .ReturnsAsync(entity);
            _mapperServiceMock.Setup(m => m.Map<RegistrantDto>(entity)).Returns(dto);
            _uowMock.Setup(u => u.GetRepository<RegistrantEntity>()).Returns(_repoMock.Object);

            var result = await _registrantService.GetById(id);

            Assert.Equal(id, result.Id);
        }

    }
}
