using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WalletAPI.Domain.Entities;
using WalletAPI.Domain.Interfaces;
using WalletAPI.Infrastructure.Data;

namespace WalletAPI.Infrastructure.Auth
{
    public class JwtService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtService(IConfiguration configuration, AppDbContext context)
        {
            _configuration = configuration;
            _context = context;
            _key = configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key");
            _issuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer");
            _audience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience");
        }

        public string GenerateToken(string userId, string email)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User?> GetUser(string email)
        {
            var result = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return result;
        }
    }
}
