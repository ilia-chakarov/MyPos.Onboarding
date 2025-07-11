using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entities;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories
{
    public class RegistrantRepository : Repository<Registrant>, IRegistrantRepository
    {
        public RegistrantRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Registrant>> GetAllRegistrantsWithWalletsAndUsersAsync()
        {
            return await _context.Registrants.Include(r => r.Wallets).Include(r => r.Users).ToListAsync();
        }
    }
}
