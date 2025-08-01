﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;
using WebAPI.Entities;
using WebAPI.Exceptions;
using WebAPI.Services.Interfaces;
using WebAPI.UnitOfWork;
using Microsoft.AspNetCore.Http;

namespace WebAPI.Services
{
    public class RegistrantService : IRegistrantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RegistrantService(IUnitOfWork uow, IMapper mapper)
        {
            _unitOfWork = uow;
            _mapper = mapper;
        }

        public async Task<RegistrantDto> CreateRegistrant(CreateRegistrantDto dto, CancellationToken cancellationToken = default)
        {
            var registrant = _mapper.Map<RegistrantEntity>(dto);

            registrant.DateCreated = DateTime.Now;

            await _unitOfWork.GetRepository<RegistrantEntity>().AddAsync(registrant, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<RegistrantDto>(registrant);
        }

        public async Task<RegistrantDto> DeleteRegistrant(int id, CancellationToken cancellationToken = default)
        {
            var registrant = await _unitOfWork.GetRepository<RegistrantEntity>().GetByIdAsync(id, cancellationToken);

            if (registrant == null)
                throw new MyPosApiException($"Registrant with id {id} not found", StatusCodes.Status404NotFound);

            _unitOfWork.GetRepository<RegistrantEntity>().Delete(registrant);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<RegistrantDto>(registrant);
        }

        public async Task<IEnumerable<RegistrantDto>> GetAll(int pageNumber, int pageSize, 
            Func<IQueryable<RegistrantEntity>, IQueryable<RegistrantEntity>>? filter = null,
            CancellationToken cancellationToken = default)
        {
            var result = await _unitOfWork.GetRepository<RegistrantEntity>().GetAllAsync<RegistrantDto>(
                mapper: _mapper,
                filter: filter, pageNumber: pageNumber, pageSize: pageSize, disableTracking: false, 
                cancellationToken: cancellationToken);

            return result;
        }

        public async Task<IEnumerable<RegistrantWithAllWalletsAndUsersDto>> GetAllWithWalletsAndUsers(int pageNumber, int pageSize, 
            Func<IQueryable<RegistrantEntity>, IQueryable<RegistrantEntity>>? filter = null,
            CancellationToken cancellationToken = default)
        {
            var res = await _unitOfWork.GetRepository<RegistrantEntity>().GetAllAsync<RegistrantWithAllWalletsAndUsersDto>(mapper: _mapper,
                filter: filter,include: q => q.Include(r => r.Wallets).Include(r => r.Users), 
                pageNumber: pageNumber, pageSize: pageSize, disableTracking: false, cancellationToken: cancellationToken);

            return res;
        }

        public async Task<RegistrantDto> GetById(int id, CancellationToken cancellationToken = default)
        {
            var registrant = await _unitOfWork.GetRepository<RegistrantEntity>().GetSingleAsync(q =>
             q.Where(u => u.Id == id), cancellationToken);

            if (registrant == null)
                throw new MyPosApiException($"Registrant with id {id} not found", StatusCodes.Status404NotFound);

            return _mapper.Map<RegistrantDto>(registrant);
        }

        public async Task<RegistrantDto> UpdateRegistrant(int id, CreateRegistrantDto dto, CancellationToken cancellationToken = default)
        {
            var registrant = await _unitOfWork.GetRepository<RegistrantEntity>().GetByIdAsync(id, cancellationToken);

            if (registrant == null)
                throw new MyPosApiException($"Registrant with id {id} not found", StatusCodes.Status404NotFound);

            _mapper.Map(dto, registrant);

            _unitOfWork.GetRepository<RegistrantEntity>().Update(registrant);
            await _unitOfWork.SaveChangesAsync(cancellationToken);


            return _mapper.Map<RegistrantDto>(registrant);
        }
    }
}
