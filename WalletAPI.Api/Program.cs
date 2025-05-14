using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WalletAPI.Application.Interfaces;
using WalletAPI.Application.Services;
using WalletAPI.Domain.Interfaces;
using WalletAPI.Infrastructure.Auth;
using WalletAPI.Infrastructure.Data;

namespace WalletAPI.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddScoped<IAuthAppService, AuthAppService>();
            builder.Services.AddScoped<IAuthService, JwtService>();
            builder.Services.AddAuthorization();
            builder.Services.AddScoped<JwtService>();

            //JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            //Swagger + Anotações + Suporte a JWT
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Wallet API",
                    Version = "v1",
                    Description = "API para gerenciamento de carteiras digitais"
                });

                c.EnableAnnotations(); // [SwaggerOperation]

                // JWT no Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header usando o esquema Bearer. Ex: 'Bearer {token}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            //Banco de Dados (PostgreSQL com fallback para SQLite)
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

            //Middlewares
            app.UseAuthentication();
            app.UseAuthorization();

            //Swagger UI
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wallet API v1");
                    c.RoutePrefix = string.Empty; // Swagger em https://localhost:5001/
                });
            }

            app.UseHttpsRedirection();
            app.MapControllers();

            // Banco: EnsureCreated para SQLite, Migrations para PostgreSQL
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                if (dbContext.Database.IsSqlite())
                {
                    dbContext.Database.EnsureCreated();
                }
                else
                {
                    dbContext.Database.Migrate();
                }
            }

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
