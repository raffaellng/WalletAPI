using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WalletAPI.Domain.Entities;

namespace WalletAPI.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Wallet> Wallets => Set<Wallet>();
        public DbSet<Transaction> Transactions => Set<Transaction>();

        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tabela User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();

                entity.HasOne(e => e.Wallet)
                              .WithOne(w => w.User)
                              .HasForeignKey<Wallet>(w => w.UserId)
                              .OnDelete(DeleteBehavior.Cascade);
            });

            // Tabela Wallet
            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Balance).HasPrecision(18, 2);
            });

            // Tabela Transaction
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasPrecision(18, 2);
                entity.Property(e => e.CreatedAt).IsRequired();

                // Relacionamento opcional com usuários (sender e receiver)
                entity.HasOne(t => t.SenderUser)
                      .WithMany()
                      .HasForeignKey(t => t.SenderUserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.ReceiverUser)
                      .WithMany()
                      .HasForeignKey(t => t.ReceiverUserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

    }
}
