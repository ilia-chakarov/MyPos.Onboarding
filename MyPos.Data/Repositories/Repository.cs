using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Repositories.Interfaces;
using AutoMapper;
using WebAPI.Entities;

namespace WebAPI.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<TEntity?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task AddAsync(TEntity entity) => await _dbSet.AddAsync(entity);

        public void Update(TEntity entity) => _dbSet.Update(entity);

        public void Delete(TEntity entity) => _dbSet.Remove(entity);

        public IQueryable<TEntity> Query() => _dbSet.AsQueryable();

        public async Task<TEntity?> GetSingleAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> filter)
        {
            return await filter(_dbSet).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TResult>> GetAllAsync<TResult>(
            IMapper mapper,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? filter = null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, 
            int? pageNumber = null,
            int? pageSize = null,
            bool disableTracking = true)
        {
            var query = _dbSet.AsQueryable();

            if (disableTracking)
                query = query.AsNoTracking();
            else 
                query = query.AsTracking();

            if(include != null)
                query = include(query);

            if(filter != null)
                query = filter(query);

            if(orderBy != null)
                query = orderBy(query);

            var projected = mapper.ProjectTo<TResult>(query);

            if(pageNumber.HasValue && pageSize.HasValue)
                projected = projected.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);

            return await projected.ToListAsync();
        }

    }
}
