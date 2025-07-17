using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public IQueryable<T> Query() => _dbSet.AsQueryable();

        public async Task<T?> GetSingleAsync(Func<IQueryable<T>, IQueryable<T>> filter)
        {
            return await filter(_dbSet).FirstOrDefaultAsync();
        }
    }
}
