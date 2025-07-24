using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;
using WebAPI.Entities;
using WebAPI.Exceptions;
using WebAPI.Services.Interfaces;
using WebAPI.UnitOfWork;
using Microsoft.AspNetCore.Http;

namespace WebAPI.Services
{
    public class UserAccessControlService : IUserAccessControlService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserAccessControlService(IUnitOfWork uow, IMapper mapper)
        {
            _unitOfWork = uow;
            _mapper = mapper;
        }
        public async Task<CreateUserAccessControlDto> CreateUAC(CreateUserAccessControlDto dto, CancellationToken cancellationToken = default)
        {
            var user = await _unitOfWork.GetRepository<UserEntity>().GetByIdAsync(dto.UserId, cancellationToken);
            if (user == null) 
                throw new MyPosApiException($"User with userId {dto.UserId} not found",
                    StatusCodes.Status404NotFound);

            var wallet = await _unitOfWork.GetRepository<WalletEntity>().GetByIdAsync(dto.WalletId, cancellationToken);
            if (wallet == null)
                throw new MyPosApiException($"Wallet with walletId {dto.WalletId} not found",
                    StatusCodes.Status404NotFound);

            var uac = _mapper.Map<UserAccessControlEntity>(dto);

            await _unitOfWork.GetRepository<UserAccessControlEntity>().AddAsync(uac, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CreateUserAccessControlDto>(uac);

        }

        public async Task<CreateUserAccessControlDto> DeleteUAC(int userId, int walletId, CancellationToken cancellationToken = default)
        {
            var uac = await _unitOfWork.GetRepository<UserAccessControlEntity>().GetSingleAsync(query =>
                query.Where(uac => uac.UserId == userId && uac.WalletId == walletId), cancellationToken);

            if (uac == null)
                throw new MyPosApiException($"User access control with userId {userId} and wallet id {walletId} not found",
                    StatusCodes.Status404NotFound);

            _unitOfWork.GetRepository<UserAccessControlEntity>().Delete(uac);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CreateUserAccessControlDto>(uac);
        }

        public async Task<IEnumerable<CreateUserAccessControlDto>> GetAll(int pageNumber, int pageSize,
            Func<IQueryable<UserAccessControlEntity>, IQueryable<UserAccessControlEntity>>? filter = null,
            CancellationToken cancellationToken = default)
        {
            var res = await _unitOfWork.GetRepository<UserAccessControlEntity>().GetAllAsync<CreateUserAccessControlDto>(
                mapper: _mapper, filter: filter, pageNumber: pageNumber, pageSize: pageSize, disableTracking: false,
                cancellationToken: cancellationToken);

            return res;
        }

        public async Task<CreateUserAccessControlDto> GetById(int userId, int walletId, CancellationToken cancellationToken = default)
        {
            var uac = await _unitOfWork.GetRepository<UserAccessControlEntity>().GetSingleAsync(query =>
                query.Where(uac => uac.UserId == userId && uac.WalletId == walletId), cancellationToken);

            if (uac == null)
                throw new MyPosApiException($"User access control with userId {userId} and wallet id {walletId} not found",
                    StatusCodes.Status404NotFound);

            return _mapper.Map<CreateUserAccessControlDto>(uac);
        }

        public async Task<CreateUserAccessControlDto> UpdateUAC(int userId, int walletId, CreateUserAccessControlDto dto,
            CancellationToken cancellationToken = default)
        {
            var uac = await _unitOfWork.GetRepository<UserAccessControlEntity>().GetSingleAsync(query =>
                query.Where(uac => uac.UserId == userId && uac.WalletId == walletId), cancellationToken);

            if (uac == null)
                throw new MyPosApiException($"User access control with userId {userId} and wallet id {walletId} not found",
                    StatusCodes.Status404NotFound);

            var usr = await _unitOfWork.GetRepository<UserEntity>().GetSingleAsync(q => q.Where(u => u.Id == dto.UserId), cancellationToken);
            if (usr == null)
                throw new MyPosApiException($"User with id {dto.UserId} not found", StatusCodes.Status404NotFound);

            var wal = await _unitOfWork.GetRepository<WalletEntity>().GetSingleAsync(q => q.Where(w => w.Id == dto.WalletId), cancellationToken);
            if (wal == null)
                throw new MyPosApiException($"Wallet with id {dto.WalletId} not found", StatusCodes.Status404NotFound);


            if (userId == dto.UserId && walletId == dto.WalletId)
            {
                uac.AccessLevel = dto.AccessLevel;
                _unitOfWork.GetRepository<UserAccessControlEntity>().Update(uac);
            }
            else
            {
                _unitOfWork.GetRepository<UserAccessControlEntity>().Delete(uac);

                var newUac = _mapper.Map<UserAccessControlEntity>(dto);

                await _unitOfWork.GetRepository<UserAccessControlEntity>().AddAsync(newUac, cancellationToken);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Get the new dto anyway if no exception is thrown
            return dto;
        }

    }
}
