using WalletAPI.Domain.Entities;
using WalletAPI.Infrastructure.Data;

namespace WalletAPI.Infrastructure.Seed
{
    public class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            if (context.Users.Any()) return;

            var users = new List<User>();

            var userAdmin = new User(
                    name: $"admin",
                    email: $"admin@admin.com",
                    passwordHash: BCrypt.Net.BCrypt.HashPassword("admin@123")
                );

            userAdmin.Wallet.AddBalance(100);

            users.Add(userAdmin);

            for (int i = 1; i <= 5; i++)
            {
                var user = new User(
                    name: $"Usuário {i}",
                    email: $"user{i}@email.com",
                    passwordHash: BCrypt.Net.BCrypt.HashPassword("123456")
                );

                user.Wallet.AddBalance(100 * i);

                users.Add(user);
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            var transactions = new List<Transaction>
        {
            new Transaction(users[0].Id, users[1].Id, 50),
            new Transaction(users[1].Id, users[2].Id, 70),
            new Transaction(users[2].Id, users[3].Id, 30),
            new Transaction(users[3].Id, users[0].Id, 20),
        };

            context.Transactions.AddRange(transactions);
            context.SaveChanges();
        }
    }
}
