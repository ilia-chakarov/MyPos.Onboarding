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
    }
}
