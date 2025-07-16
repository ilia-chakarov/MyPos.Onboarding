using AutoMapper;
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
        private readonly IMapper _mapper;

        public UserAccessControlService(IUnitOfWork uow, IMapper mapper)
        {
            _unitOfWork = uow;
            _mapper = mapper;
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

            var uac = _mapper.Map<UserAccessControlEntity>(dto);

            await _unitOfWork.GetRepository<UserAccessControlEntity>().AddAsync(uac);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CreateUserAccessControlDto>(uac);

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

            return _mapper.Map<CreateUserAccessControlDto>(uac);
        }

        public Task<CreateUserAccessControlDto> DeleteUAC(int userId, int walletId, CreateUserAccessControlDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CreateUserAccessControlDto>> GetAll(int pageNumber, int pageSize, Func<IQueryable<UserAccessControlEntity>, IQueryable<UserAccessControlEntity>>? filter = null)
        {
            var query = _unitOfWork.GetRepository<UserAccessControlEntity>().Query().Skip((pageNumber - 1) * pageSize).Take(pageSize);

            if(filter != null)
                query = filter(query);

            // No mapper is faster
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

            return _mapper.Map<CreateUserAccessControlDto>(uac);
        }

        public async Task<CreateUserAccessControlDto> UpdateUAC(int userId, int walletId, CreateUserAccessControlDto dto)
        {
            var uac = await _unitOfWork.GetRepository<UserAccessControlEntity>().GetSingleAsync(query =>
                query.Where(uac => uac.UserId == userId && uac.WalletId == walletId));

            if (uac == null)
                throw new MyPosApiException($"User access control with userId {userId} and wallet id {walletId} not found",
                    StatusCodes.Status404NotFound);

            var usr = await _unitOfWork.GetRepository<UserEntity>().GetSingleAsync(q => q.Where(u => u.Id == dto.UserId));
            if (usr == null)
                throw new MyPosApiException($"User with id {dto.UserId} not found", StatusCodes.Status404NotFound);

            var wal = await _unitOfWork.GetRepository<WalletEntity>().GetSingleAsync(q => q.Where(w => w.Id == dto.WalletId));
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

                await _unitOfWork.GetRepository<UserAccessControlEntity>().AddAsync(newUac);
            }

            await _unitOfWork.SaveChangesAsync();

            // Get the new dto anyway if no exception is thrown
            return dto;
        }

    }
}
