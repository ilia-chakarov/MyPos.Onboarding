using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;
using WebAPI.Entities;
using WebAPI.Exceptions;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;
using WebAPI.UnitOfWork;

namespace WebAPI.Services
{
    public class UserAccessControlService : IUserAccessControlService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserAccessControlService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        public async Task<CreateUserAccessControlDto> CreateUAC(CreateUserAccessControlDto dto)
        {
            var user = await _unitOfWork.GetRepository<UserEntity>().GetByIdAsync(dto.UserId);
            if (user == null) 
                throw new MyPosApiException($"User with userId {dto.UserId} not found",
                    StatusCodes.Status404NotFound);

            var wallet = await _unitOfWork.GetRepository<WalletEntity>().GetByIdAsync(dto.WalletId);
            if (wallet == null)
                throw new MyPosApiException($"Wallet with walletId {dto.WalletId} not found",
                    StatusCodes.Status404NotFound);

            var uac = new UserAccessControlEntity
            {
                UserId = dto.UserId,
                WalletId = dto.WalletId,
                AccessLevel = dto.AccessLevel
            };

            await _unitOfWork.GetRepository<UserAccessControlEntity>().AddAsync(uac);
            await _unitOfWork.SaveChangesAsync();

            return new CreateUserAccessControlDto
            {
                UserId = uac.UserId,
                WalletId = uac.WalletId,
                AccessLevel = uac.AccessLevel,
            }
            ;

        }

        public async Task<CreateUserAccessControlDto> DeleteUAC(int userId, int walletId)
        {
            var uac = await _unitOfWork.GetRepository<UserAccessControlEntity>().GetSingleAsync(query =>
                query.Where(uac => uac.UserId == userId && uac.WalletId == walletId));

            if (uac == null)
                throw new MyPosApiException($"User access control with userId {userId} and wallet id {walletId} not found",
                    StatusCodes.Status404NotFound);

            _unitOfWork.GetRepository<UserAccessControlEntity>().Delete(uac);
            await _unitOfWork.SaveChangesAsync();

            return new CreateUserAccessControlDto
            {
                UserId = uac.UserId,
                WalletId = uac.WalletId,
                AccessLevel = uac.AccessLevel,
            }; 
        }

        public Task<CreateUserAccessControlDto> DeleteUAC(int userId, int walletId, CreateUserAccessControlDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CreateUserAccessControlDto>> GetAll(Func<IQueryable<UserAccessControlEntity>, IQueryable<UserAccessControlEntity>>? filter = null)
        {
            var query = _unitOfWork.GetRepository<UserAccessControlEntity>().Query();

            if(filter != null)
                query = filter(query);

            return await query.Select(x =>
                new CreateUserAccessControlDto
                {
                    UserId = x.UserId,
                    WalletId = x.WalletId,
                    AccessLevel = x.AccessLevel,
                }).ToListAsync();
        }

        public async Task<CreateUserAccessControlDto> GetById(int userId, int walletId)
        {
            var uac = await _unitOfWork.GetRepository<UserAccessControlEntity>().GetSingleAsync(query =>
                query.Where(uac => uac.UserId == userId && uac.WalletId == walletId));

            if (uac == null)
                throw new MyPosApiException($"User access control with userId {userId} and wallet id {walletId} not found",
                    StatusCodes.Status404NotFound);

            return new CreateUserAccessControlDto
            {
                UserId = uac.UserId,
                WalletId = uac.WalletId,
                AccessLevel = uac.AccessLevel,
            };
        }

        public async Task<CreateUserAccessControlDto> UpdateUAC(int userId, int walletId, CreateUserAccessControlDto dto)
        {
            var uac = await _unitOfWork.GetRepository<UserAccessControlEntity>().GetSingleAsync(query =>
                query.Where(uac => uac.UserId == userId && uac.WalletId == walletId));

            if (uac == null)
                throw new MyPosApiException($"User access control with userId {userId} and wallet id {walletId} not found",
                    StatusCodes.Status404NotFound);


            if (userId == dto.UserId && walletId == dto.WalletId)
            {
                uac.AccessLevel = dto.AccessLevel;
                _unitOfWork.GetRepository<UserAccessControlEntity>().Update(uac);
            }
            else
            {
                _unitOfWork.GetRepository<UserAccessControlEntity>().Delete(uac);
                var newUac = new UserAccessControlEntity
                {
                    UserId = dto.UserId,
                    WalletId = dto.WalletId,
                    AccessLevel = dto.AccessLevel,
                };
                await _unitOfWork.GetRepository<UserAccessControlEntity>().AddAsync(newUac);
            }

            await _unitOfWork.SaveChangesAsync();

            return new CreateUserAccessControlDto { UserId = userId, WalletId = walletId, AccessLevel = dto.AccessLevel, };
        }

    }
}
