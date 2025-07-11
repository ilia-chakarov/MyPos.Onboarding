using WebAPI.Entities;

namespace WebAPI.Repositories.Interfaces
{
    public interface IRegistrantRepository : IRepository<Registrant>
    {
        Task<IEnumerable<Registrant>> GetAllRegistrantsWithWalletsAndUsersAsync();
    }
}
