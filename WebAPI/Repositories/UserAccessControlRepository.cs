using WebAPI.Data;
using WebAPI.Entities;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories
{
    public class UserAccessControlRepository : Repository<UserAccessControl>, IUserAccessControlRepository
    {
        public UserAccessControlRepository(AppDbContext context) : base(context)
        {
        }
    }
}
