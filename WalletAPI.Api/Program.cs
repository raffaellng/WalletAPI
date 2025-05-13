using Microsoft.EntityFrameworkCore;
using WalletAPI.Infrastructure.Data;

namespace WalletAPI.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // Configurações de banco de dados
            var connectionStringPostgreSQL = builder.Configuration.GetConnectionString("PostgreSQL");
            var connectionStringSQLite = builder.Configuration.GetConnectionString("SQLite");

            builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
            {
                if (TestarConexaoPostgreSQL(connectionStringPostgreSQL))
                {
                    Console.WriteLine("Usando PostgreSQL...");
                    options.UseNpgsql(connectionStringPostgreSQL);
                }
                else
                {
                    Console.WriteLine("PostgreSQL falhou, usando SQLite...");
                    options.UseSqlite(connectionStringSQLite);
                }
            });


            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                if (dbContext.Database.IsSqlite())
                {
                    // Isso cria o banco caso ainda não exista, útil para SQLite
                    dbContext.Database.EnsureCreated();
                }
                else
                {
                    // Para PostgreSQL, use migrations
                    dbContext.Database.Migrate();
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static bool TestarConexaoPostgreSQL(string connectionString)
        {
            try
            {
                using var connection = new Npgsql.NpgsqlConnection(connectionString);
                connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao conectar ao PostgreSQL: {ex.Message}");
                return false;
            }
        }

    }
}
