using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;
using WebAPI.Entities;
using WebAPI.Exceptions;
using WebAPI.Services.Interfaces;
using WebAPI.UnitOfWork;

namespace WebAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AccountService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }
        public async Task<AccountDto> GetById(int id)
        {
            var account = await _unitOfWork.GetRepository<AccountEntity>().GetSingleAsync(query =>
                query.Where(
                    a => a.Id == id
                    ));

            if (account == null)
                throw new MyPosApiException($"Account with id {id} not found", StatusCodes.Status404NotFound);

            var accountDto = new AccountDto
            {
                Id = account.Id,
                DateCreated = account.DateCreated,
                Currency = account.Currency,
                Balance = account.Balance,
                IBAN = account.IBAN,
                AccountName = account.AccountName,
                LastOperationDT = account.LastOperationDT,
                BalanceInEuro = account.BalanceInEuro,
                WalletId = account.WalletId,
            };

            return accountDto;
        }

        public async Task<IEnumerable<AccountDto>> GetAll(Func<IQueryable<AccountEntity>, IQueryable<AccountEntity>>? filter = null)
        {
            var query = _unitOfWork.GetRepository<AccountEntity>().Query();

            if (filter != null)
                query = filter(query);

            return await query.Select(x =>
                new AccountDto
                {
                    Id = x.Id,
                    DateCreated = x.DateCreated,
                    Currency = x.Currency,
                    Balance = x.Balance,
                    IBAN = x.IBAN,
                    AccountName = x.AccountName,
                    LastOperationDT = x.LastOperationDT,
                    BalanceInEuro = x.BalanceInEuro,
                    WalletId = x.WalletId,
                }
                ).ToListAsync();
        }

        public async Task<AccountDto> CreateAccount(CreateAccountDto dto)
        {
            var wallet = await _unitOfWork.GetRepository<WalletEntity>().GetSingleAsync(query =>
                query.Where(w => w.Id == dto.WalletId
            ));
            if (wallet == null)
                throw new MyPosApiException($"Wallet with id {dto.WalletId} does not exist", StatusCodes.Status404NotFound);

            var account = new AccountEntity
            {
                DateCreated = DateTime.Now,
                Currency = dto.Currency,
                Balance = dto.Balance,
                IBAN = dto.IBAN,
                AccountName = dto.AccountName,
                LastOperationDT = DateTime.Now,
                BalanceInEuro = dto.BalanceInEuro,
                WalletId = dto.WalletId,
            };

            await _unitOfWork.GetRepository<AccountEntity>().AddAsync(account);
            await _unitOfWork.SaveChangesAsync();

            return new AccountDto
                {
                    Id = account.Id,
                    DateCreated = account.DateCreated,
                    Currency = account.Currency,
                    Balance = account.Balance,
                    IBAN = account.IBAN,
                    AccountName = account.AccountName,
                    LastOperationDT = account.LastOperationDT,
                    BalanceInEuro = account.BalanceInEuro,
                    WalletId = account.WalletId,
                };
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

            return new AccountDto
            {
                Id = account.Id,
                DateCreated = account.DateCreated,
                Currency = account.Currency,
                Balance = account.Balance,
                IBAN = account.IBAN,
                AccountName = account.AccountName,
                LastOperationDT = account.LastOperationDT,
                BalanceInEuro = account.BalanceInEuro,
                WalletId = account.WalletId,
            };
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


            account.Currency = dto.Currency;
            account.Balance = dto.Balance;
            account.IBAN = dto.IBAN;
            account.AccountName = dto.AccountName;
            account.BalanceInEuro = dto.BalanceInEuro;
            account.WalletId = dto.WalletId;

            _unitOfWork.GetRepository<AccountEntity>().Update(account);
            await _unitOfWork.SaveChangesAsync();

            return new AccountDto
            {
                Id = account.Id,
                DateCreated = account.DateCreated,
                Currency = account.Currency,
                Balance = account.Balance,
                IBAN = account.IBAN,
                AccountName = account.AccountName,
                LastOperationDT = account.LastOperationDT,
                BalanceInEuro = account.BalanceInEuro,
                WalletId = account.WalletId,
            };
        }
    }
}
