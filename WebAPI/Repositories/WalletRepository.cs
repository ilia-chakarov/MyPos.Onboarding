using WebAPI.Data;
using WebAPI.Entities;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories
{
    public class WalletRepository : Repository<WalletEntity>, IWalletRepository
    {
        public WalletRepository(AppDbContext context) : base(context)
        {
        }
    }
}
