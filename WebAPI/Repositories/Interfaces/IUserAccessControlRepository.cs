using WebAPI.Entities;

namespace WebAPI.Repositories.Interfaces
{
    public interface IUserAccessControlRepository : IRepository<UserAccessControlEntity>
    {
        Task<UserAccessControlEntity?> GetByUserAndWalletAsync(int userId, int walletId);
    }
}
