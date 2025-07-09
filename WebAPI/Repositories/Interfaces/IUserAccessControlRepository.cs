using WebAPI.Entities;

namespace WebAPI.Repositories.Interfaces
{
    public interface IUserAccessControlRepository : IRepository<UserAccessControl>
    {
        Task<UserAccessControl?> GetByUserAndWalletAsync(int userId, int walletId);
    }
}
