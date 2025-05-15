using WalletAPI.Application.DTOs.Auth;
using WalletAPI.Application.Interfaces;
using WalletAPI.Domain.Interfaces;

namespace WalletAPI.Application.Services
{
    public class AuthAppService : IAuthAppService
    {
        private readonly IAuthService _authService;

        public AuthAppService(IAuthService authService)
        {
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
