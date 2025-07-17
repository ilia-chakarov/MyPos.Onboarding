using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;

namespace WebAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<WalletEntity> Wallets => Set<WalletEntity>();
        public DbSet<AccountEntity> Accounts => Set<AccountEntity>();
        public DbSet<RegistrantEntity> Registrants => Set<RegistrantEntity>();
        public DbSet<UserAccessControlEntity> UserAccessControls => Set<UserAccessControlEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserAccessControlEntity>()
                .HasKey(uac => new { uac.UserId, uac.WalletId });

            modelBuilder.Entity<UserAccessControlEntity>()
                .HasOne(uac => uac.User)
                .WithMany(u => u.AccessControls)
                .HasForeignKey(uac => uac.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserAccessControlEntity>()
                .HasOne(uac => uac.Wallet)
                .WithMany(w => w.AccessControls)
                .HasForeignKey(uac => uac.WalletId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AccountEntity>()
                .Property(a => a.Balance)
                .HasPrecision(18, 2);

            modelBuilder.Entity<AccountEntity>()
                .Property(a => a.BalanceInEuro)
                .HasPrecision(18, 2);
        }
    }
}
