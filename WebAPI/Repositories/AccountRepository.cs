using WebAPI.Data;
using WebAPI.Entities;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories
{
    public class AccountRepository : Repository<AccountEntity>, IAccountRepository
    {
        public AccountRepository(AppDbContext context) : base(context)
        {
        }
    }
}
