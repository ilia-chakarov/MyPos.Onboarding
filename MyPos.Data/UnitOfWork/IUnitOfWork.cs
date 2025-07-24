using WebAPI.Repositories;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : class;


        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
