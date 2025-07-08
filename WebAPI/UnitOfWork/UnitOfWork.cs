using WebAPI.Data;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

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

        
        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
