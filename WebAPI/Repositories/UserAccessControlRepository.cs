using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entities;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories
{
    public class UserAccessControlRepository : Repository<UserAccessControlEntity>, IUserAccessControlRepository
    {
        public UserAccessControlRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<UserAccessControlEntity?> GetByUserAndWalletAsync(int userId, int walletId)
        {
            return await _dbSet.FirstOrDefaultAsync(uac =>
                uac.UserId == userId &&  uac.WalletId == walletId);
        }
    }
}
