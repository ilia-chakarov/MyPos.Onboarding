using WebAPI.DTOs;
using WebAPI.Entities;

namespace WebAPI.Services.Interfaces
{
    public interface IWalletService
    {
        Task<IEnumerable<WalletDto>> GetAll(Func<IQueryable<Wallet>, IQueryable<Wallet>>? filter = null);

        Task<WalletDto> CreateWallet(CreateWalletDto dto);

        Task<WalletDto> GetById(int id);

        Task<WalletDto> UpdateWallet(int id, CreateWalletDto dto);
        Task<WalletDto> DeleteWallet(int id);
    }
}
