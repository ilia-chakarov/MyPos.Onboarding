using WebAPI.Repositories.Interfaces;

namespace WebAPI.UnitOfWork
{
    public interface IUnitOfWork
    {
        IAccountRepository AccountRepository { get; }
        IRegistrantRepository RegistrantRepository { get; }
        IUserAccessControlRepository UserAccessControlRepository { get; }
        IUserRepository UserRepository { get; }
        IWalletRepository WalletRepository { get; }

        Task<int> SaveChangesAsync();

    }
}
