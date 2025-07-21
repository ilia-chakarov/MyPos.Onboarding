using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using WebAPI.Entities;

namespace WebAPI.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TResult>> GetAllAsync<TResult>(
            IMapper mapper,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            int? pageNumber = null,
            int? pageSize = null,
            bool disableTracking = true);

        IQueryable<TEntity> Query();
        Task<TEntity?> GetSingleAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> filter);
        Task<TEntity?> GetByIdAsync(int id);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
