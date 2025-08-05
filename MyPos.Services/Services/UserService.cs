using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyPos.Services.DTOs;
using MyPos.Services.DTOs.FilterDTOs;
using WebAPI.DTOs;
using WebAPI.Entities;
using WebAPI.Exceptions;
using WebAPI.Services.Interfaces;
using WebAPI.UnitOfWork;

namespace WebAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork uow, IPasswordHasher<UserEntity> passwordHasher,
            IMapper mapper)
        {
            _unitOfWork = uow;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }


        public async Task<UserDto> CreateUser(CreateUserDto dto, CancellationToken cancellationToken = default)
        {
            var registrant = await _unitOfWork.GetRepository<RegistrantEntity>().GetByIdAsync(dto.RegistrantId, cancellationToken);

            if (registrant == null)
                throw new MyPosApiException($"Registrant with ID {dto.RegistrantId} not found", StatusCodes.Status404NotFound);

            var user = _mapper.Map<UserEntity>(dto);

            user.Password = _passwordHasher.HashPassword(user, dto.Password);

            await _unitOfWork.GetRepository<UserEntity>().AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync();

            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        public async Task<IEnumerable<UserDto>> GetAll(int pageNumber, int pageSize, 
            Func<IQueryable<UserEntity>, IQueryable<UserEntity>>? filter = null, 
            CancellationToken cancellationToken = default)
        {
            var res = await _unitOfWork.GetRepository<UserEntity>().GetAllAsync<UserDto>(
                mapper: _mapper, filter: filter, pageNumber: pageNumber, pageSize: pageSize, disableTracking: false,
                cancellationToken: cancellationToken);

            return res;

        }

        public async Task<CountedDto<UserDetailedDto>> GetAllCounted(int pageNumber, int pageSize, UserFilterDto? filter = null, CancellationToken cancellationToken = default)
        {
            Func<IQueryable<UserEntity>, IQueryable<UserEntity>>? filterExpression = null;

            if (filter != null)
            {
                filterExpression = u =>
                    u.Where(us =>
                        (string.IsNullOrEmpty(filter.Username) || us.Username.Contains(filter.Username)) &&
                        (string.IsNullOrEmpty(filter.RegistrantName) || us.Registrant.DisplayName.Contains(filter.RegistrantName)) &&
                        (!filter.UserId.HasValue || us.Id == filter.UserId));
            }

            var detailedUsers = await _unitOfWork.GetRepository<UserEntity>().GetAllCountedAsync<UserDetailedDto>(
                mapper: _mapper, 
                filter: filterExpression,
                pageNumber: pageNumber, pageSize: pageSize, disableTracking: false,
                cancellationToken: cancellationToken, include: i => i.Include(r => r.Registrant));

            var result = new CountedDto<UserDetailedDto>
            {
                CountDto = new CountDto
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = detailedUsers.totalCount
                },
                Items = detailedUsers.items
            };

            return result;
        }

        public async Task<UserDto> GetById(int id, CancellationToken cancellationToken = default)
        {
            var usr = await _unitOfWork.GetRepository<UserEntity>().GetSingleAsync(query => 
                query.Where(u => u.Id == id), cancellationToken);

            if (usr == null)
                throw new MyPosApiException($"No user with id {id} found", StatusCodes.Status404NotFound);

            return _mapper.Map<UserDto>(usr);
        }

        public async Task<UserDto> UpdateUser(int id, CreateUserDto dto, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.GetRepository<UserEntity>().GetSingleAsync(q =>
                q.Where(u => u.Id == id), cancellationToken
            );

            if (user == null)
                throw new MyPosApiException($"User with id {id} found", StatusCodes.Status404NotFound);

            var currentPassword = user.Password;

            var registrant = await _unitOfWork.GetRepository<RegistrantEntity>().GetSingleAsync(q =>
                q.Where(r => r.Id == dto.RegistrantId), cancellationToken
                );

            if (registrant == null)
                throw new MyPosApiException($"Registrant with ID {dto.RegistrantId} not found", StatusCodes.Status404NotFound);

            if(!string.IsNullOrEmpty(dto.Username))
                _mapper.Map(dto, user);

            if (!string.IsNullOrEmpty(dto.Password))
                user.Password = _passwordHasher.HashPassword(user, dto.Password);
            else
                user.Password = currentPassword;

                _unitOfWork.GetRepository<UserEntity>().Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserDto>(user);
        }
        public async Task<UserDto> DeleteUser(int id, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.GetRepository<UserEntity>().GetSingleAsync(q =>
                q.Where(u => u.Id == id), cancellationToken);

            if (user == null)
                throw new MyPosApiException($"User with id {id} found", StatusCodes.Status404NotFound);

            _unitOfWork.GetRepository<UserEntity>().Delete(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserDto>(user);
        }

        
    }
}
