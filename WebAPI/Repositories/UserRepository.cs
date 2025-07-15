using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entities;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories
{
    public class UserRepository : Repository<UserEntity>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public Task<UserEntity?> GetByUsernameAsync(string username)
        {
            return _dbSet.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
