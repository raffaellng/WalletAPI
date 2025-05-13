using Microsoft.EntityFrameworkCore;
using WalletAPI.Domain.Entities;

namespace WalletAPI.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Wallet> Wallets => Set<Wallet>();
        public DbSet<Transaction> Transactions => Set<Transaction>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.PasswordHash).IsRequired();

                entity.HasOne(u => u.Wallet)
                      .WithOne(w => w.User)
                      .HasForeignKey<Wallet>(w => w.UserId);
            });

            // Wallet
            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.HasKey(w => w.Id);
                entity.Property(w => w.Balance).IsRequired().HasColumnType("decimal(18,2)");

                entity.HasOne(w => w.User)
                      .WithOne(u => u.Wallet)
                      .HasForeignKey<Wallet>(w => w.UserId);

                entity.HasMany(w => w.TransactionsAsSender)
                      .WithOne(t => t.SenderWallet)
                      .HasForeignKey(t => t.SenderWalletId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(w => w.TransactionsAsReceiver)
                      .WithOne(t => t.ReceiverWallet)
                      .HasForeignKey(t => t.ReceiverWalletId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Transaction
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Amount).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(t => t.CreatedAt).IsRequired();
            });
        }
    }
}
