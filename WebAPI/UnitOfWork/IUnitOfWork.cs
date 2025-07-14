using WebAPI.Repositories;
using WebAPI.Repositories.Interfaces;

namespace WebAPI.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : class;

        IAccountRepository AccountRepository { get; }
        IRegistrantRepository RegistrantRepository { get; }
        IUserAccessControlRepository UserAccessControlRepository { get; }
        IUserRepository UserRepository { get; }
        IWalletRepository WalletRepository { get; }

        Task<int> SaveChangesAsync();

    }
}
