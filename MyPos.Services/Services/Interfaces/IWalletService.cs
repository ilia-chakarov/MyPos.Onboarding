using WebAPI.DTOs;
using WebAPI.Entities;

namespace WebAPI.Services.Interfaces
{
    public interface IWalletService
    {
        Task<IEnumerable<WalletDto>> GetAll(int pageNumber, int pageSize, 
            Func<IQueryable<WalletEntity>, IQueryable<WalletEntity>>? filter = null,
            CancellationToken cancellationToken = default);

        Task<WalletDto> CreateWallet(CreateWalletDto dto, CancellationToken cancellationToken = default);

        Task<WalletDto> GetById(int id, CancellationToken cancellationToken = default);

        Task<WalletDto> UpdateWallet(int id, CreateWalletDto dto, CancellationToken cancellationToken = default);
        Task<WalletDto> DeleteWallet(int id, CancellationToken cancellationToken = default);
    }
}
