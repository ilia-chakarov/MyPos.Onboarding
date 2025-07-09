using WebAPI.Entities;

namespace WebAPI.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
    }
}
