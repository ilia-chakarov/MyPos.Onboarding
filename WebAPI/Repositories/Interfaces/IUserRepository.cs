using WebAPI.Entities;

namespace WebAPI.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        Task<UserEntity?> GetByUsernameAsync(string username);
    }
}
