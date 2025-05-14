using Microsoft.EntityFrameworkCore;
using WalletAPI.Application.DTOs.Auth;
using WalletAPI.Application.Interfaces;
using WalletAPI.Domain.Interfaces;
using WalletAPI.Infrastructure.Data;

namespace WalletAPI.Application.Services
{
    public class AuthAppService : IAuthAppService
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;

        public AuthAppService(AppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _authService.GetUser(request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Usuário ou senha inválidos.");

            var token = _authService.GenerateToken(user.Id.ToString(), user.Email);

            return new LoginResponseDto { Token = token };
        }
    }
}
