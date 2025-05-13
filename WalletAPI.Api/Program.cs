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

            var connectionStringPostgreSQL = builder.Configuration.GetConnectionString("PostgreSQL");
            var connectionStringSQLite = builder.Configuration.GetConnectionString("SQLite");

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                try
                {
                    Console.WriteLine("Tentando conectar ao PostgreSQL...");
                    options.UseNpgsql(connectionStringPostgreSQL);
                }
                catch (Exception)
                {
                    Console.WriteLine("Falha na conexão com PostgreSQL, alternando para SQLite...");
                    options.UseSqlite(connectionStringSQLite);
                }
            });

            var app = builder.Build();

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
    }
}
