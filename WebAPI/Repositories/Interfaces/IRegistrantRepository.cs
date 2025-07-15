using WebAPI.Entities;

namespace WebAPI.Repositories.Interfaces
{
    public interface IRegistrantRepository : IRepository<RegistrantEntity>
    {
        Task<IEnumerable<RegistrantEntity>> GetAllRegistrantsWithWalletsAndUsersAsync();
    }
}
