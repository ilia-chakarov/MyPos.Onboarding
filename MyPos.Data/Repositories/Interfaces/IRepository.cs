using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using WebAPI.Entities;

namespace WebAPI.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<TResult>> GetAllAsync<TResult>(
            IMapper mapper,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null!,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null!,
            int? pageNumber = null,
            int? pageSize = null,
            bool disableTracking = true,
            CancellationToken cancellationToken = default);
        Task<(IEnumerable<TResult> items, int totalCount)> GetAllCountedAsync<TResult>(
            IMapper mapper,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null!,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null!,
            int? pageNumber = null,
            int? pageSize = null,
            bool disableTracking = true,
            CancellationToken cancellationToken = default);
        Task<TEntity?> GetSingleAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> filter, CancellationToken cancellationToken = default);
        Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
