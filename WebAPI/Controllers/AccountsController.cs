using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using WebAPI.DTOs;
using WebAPI.Entities;
using WebAPI.UnitOfWork;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountsController(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAccountDto dto)
        {
            var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(dto.WalletId);
            if(wallet == null)
                return BadRequest($"Wallet with id {dto.WalletId} does not exist");

            var account = new Account
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

            await _unitOfWork.AccountRepository.AddAsync(account);
            await _unitOfWork.SaveChangesAsync();

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

            return CreatedAtAction(nameof(GetById), new {id = account.Id}, accountDto);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var account = await _unitOfWork.AccountRepository.GetByIdAsync(id);

            if(account == null)
                return NotFound();

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

            return Ok(accountDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _unitOfWork.AccountRepository.GetAllAsync();

            var accountDtos = accounts.Select(x => new AccountDto
            {
                Id=x.Id,
                DateCreated = x.DateCreated,
                Currency = x.Currency,
                Balance = x.Balance,
                IBAN = x.IBAN,
                AccountName = x.AccountName,
                LastOperationDT = x.LastOperationDT,
                BalanceInEuro= x.BalanceInEuro,
                WalletId = x.WalletId,
            });

            return Ok(accountDtos);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateAccountDto dto)
        {
            var account = await _unitOfWork.AccountRepository.GetByIdAsync(id);
            if (account == null) return NotFound();

            var wallet = await _unitOfWork.WalletRepository.GetByIdAsync(dto.WalletId);
            if (wallet == null) return BadRequest($"Wallet with id {dto.WalletId} does not exist");

            account.Currency = dto.Currency;
            account.Balance = dto.Balance;
            account.IBAN = dto.IBAN;
            account.AccountName = dto.AccountName;
            account.BalanceInEuro = dto.BalanceInEuro;
            account.WalletId = dto.WalletId;

            _unitOfWork.AccountRepository.Update(account);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var account = await _unitOfWork.AccountRepository.GetByIdAsync(id);
            if (account == null) return NotFound();
            
            _unitOfWork.AccountRepository.Delete(account);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

    }
}
