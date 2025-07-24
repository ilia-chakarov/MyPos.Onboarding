using WebAPI.DTOs;
using WebAPI.Entities;

namespace WebAPI.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDto>> GetAll(int pageNumber, int pageSize,
            Func<IQueryable<AccountEntity>, IQueryable<AccountEntity>>? filter = null,
            CancellationToken cancellationToken = default);

        Task<AccountDto> CreateAccount(CreateAccountDto dto, CancellationToken cancellationToken = default);

        Task<AccountDto> GetById(int id, CancellationToken cancellationToken = default);

        Task<AccountDto> UpdateAccount(int id, CreateAccountDto dto, CancellationToken cancellationToken = default);
        Task<AccountDto> DeleteAccount(int id, CancellationToken cancellationToken = default);
    }
}
