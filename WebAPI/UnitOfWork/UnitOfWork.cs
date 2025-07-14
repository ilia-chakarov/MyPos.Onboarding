using WebAPI.Data;
using WebAPI.Repositories;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();

        public IAccountRepository AccountRepository {  get; }

        public IRegistrantRepository RegistrantRepository { get; }

        public IUserAccessControlRepository UserAccessControlRepository { get; }

        public IUserRepository UserRepository { get; }

        public IWalletRepository WalletRepository { get; }



        public UnitOfWork(AppDbContext context,
                            IAccountRepository a,
                            IRegistrantRepository r,
                            IUserAccessControlRepository uac,
                            IUserRepository u,
                            IWalletRepository w)
        {
            _context = context;
            AccountRepository = a;
            RegistrantRepository = r;
            UserAccessControlRepository = uac;
            UserRepository = u;
            WalletRepository = w;
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


        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        
    }
}
