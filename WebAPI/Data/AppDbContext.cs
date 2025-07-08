using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;

namespace WebAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<User> Users => Set<User>();
        public DbSet<Wallet> Wallets => Set<Wallet>();
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Registrant> Registrants => Set<Registrant>();
        public DbSet<UserAccessControl> UserAccessControls => Set<UserAccessControl>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserAccessControl>()
                .HasKey(uac => new { uac.UserId, uac.WalletId });

            modelBuilder.Entity<UserAccessControl>()
                .HasOne(uac => uac.User)
                .WithMany(u => u.AccessControls)
                .HasForeignKey(uac => uac.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserAccessControl>()
                .HasOne(uac => uac.Wallet)
                .WithMany(w => w.AccessControls)
                .HasForeignKey(uac => uac.WalletId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Account>()
                .Property(a => a.Balance)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Account>()
                .Property(a => a.BalanceInEuro)
                .HasPrecision(18, 2);
        }
    }
}
