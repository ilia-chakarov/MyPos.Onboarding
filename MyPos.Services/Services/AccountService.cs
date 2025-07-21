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
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountService(IUnitOfWork uow, IMapper mapper)
        {
            _unitOfWork = uow;
            _mapper = mapper;
        }
        public async Task<AccountDto> GetById(int id)
        {
            var account = await _unitOfWork.GetRepository<AccountEntity>().GetSingleAsync(query =>
                query.Where(
                    a => a.Id == id
                    ));

            if (account == null)
                throw new MyPosApiException($"Account with id {id} not found", StatusCodes.Status404NotFound);

            return _mapper.Map<AccountDto>(account);
        }

        public async Task<IEnumerable<AccountDto>> GetAll(int pageNumber, int pageSize, Func<IQueryable<AccountEntity>, IQueryable<AccountEntity>>? filter = null)
        {
            var res = await _unitOfWork.GetRepository<AccountEntity>().GetAllAsync<AccountDto>(
                mapper: _mapper, filter: filter, pageNumber: pageNumber, pageSize: pageSize, disableTracking: false);

            return res;
        }

        public async Task<AccountDto> CreateAccount(CreateAccountDto dto)
        {
            var wallet = await _unitOfWork.GetRepository<WalletEntity>().GetSingleAsync(query =>
                query.Where(w => w.Id == dto.WalletId
            ));
            if (wallet == null)
                throw new MyPosApiException($"Wallet with id {dto.WalletId} does not exist", StatusCodes.Status404NotFound);

            var account = _mapper.Map<AccountEntity>(dto);
            account.DateCreated = DateTime.Now;
            account.LastOperationDT = DateTime.Now;

            await _unitOfWork.GetRepository<AccountEntity>().AddAsync(account);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AccountDto>(account);
        }

        public async Task<AccountDto> DeleteAccount(int id)
        {
            var account = await _unitOfWork.GetRepository<AccountEntity>().GetSingleAsync(query =>
                query.Where(
                    a => a.Id == id
                    ));

            if (account == null)
                throw new MyPosApiException($"Account with id {id} not found", StatusCodes.Status404NotFound);

            _unitOfWork.GetRepository<AccountEntity>().Delete(account);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AccountDto>(account);
        }

        public async Task<AccountDto> UpdateAccount(int id, CreateAccountDto dto)
        {
            var account = await _unitOfWork.GetRepository<AccountEntity>().GetSingleAsync(query =>
                query.Where(
                    a => a.Id == id
                    ));

            if (account == null)
                throw new MyPosApiException($"Account with id {id} not found",
                    StatusCodes.Status404NotFound);

            var wallet = await _unitOfWork.GetRepository<WalletEntity>().GetSingleAsync(query =>
                query.Where(w => w.Id == dto.WalletId
            ));
            if (wallet == null)
                throw new MyPosApiException($"Wallet with id {dto.WalletId} does not exist",
                    StatusCodes.Status404NotFound);

            _mapper.Map(dto, account);

            _unitOfWork.GetRepository<AccountEntity>().Update(account);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<AccountDto>(account);
        }
    }
}
