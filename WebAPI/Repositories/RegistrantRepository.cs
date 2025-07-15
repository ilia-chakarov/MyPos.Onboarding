using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entities;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories
{
    public class RegistrantRepository : Repository<RegistrantEntity>, IRegistrantRepository
    {
        public RegistrantRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<RegistrantEntity>> GetAllRegistrantsWithWalletsAndUsersAsync()
        {
            return await _context.Registrants.Include(r => r.Wallets).Include(r => r.Users).ToListAsync();
        }
    }
}
