using WebAPI.Data;
using WebAPI.Repositories;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();

 



        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }


        public IRepository<T> GetRepository<T>() where T : class
        {
            var type = typeof(T);
            if (!_repositories.ContainsKey(type))
            {
                var repoInstance = new Repository<T>(_context);
                _repositories[type] = repoInstance;
            }
            return (IRepository<T>)_repositories[type];
        }


        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => await _context.SaveChangesAsync(cancellationToken);

        
    }
}
