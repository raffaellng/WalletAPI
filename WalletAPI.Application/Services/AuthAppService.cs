using WalletAPI.Application.DTOs.Auth.Request;
using WalletAPI.Application.DTOs.Auth.Response;
using WalletAPI.Application.Interfaces;
using WalletAPI.Domain.Exceptions;
using WalletAPI.Domain.Interfaces;
using WalletAPI.Domain.Utils;

namespace WalletAPI.Application.Services
{
    public class AuthAppService : IAuthAppService
    {
        private readonly IAuthRepository _authService;

        public AuthAppService(IAuthRepository authService)
        {
            _authService = authService;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            try
            {
                var user = await _authService.GetUser(request.Email);

                if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                    throw new ApiException(401, Mensagens.UsuarioSenhaInvalidos);

                var token = _authService.GenerateToken(user.Id.ToString(), user.Email);

                return new LoginResponseDto { Token = token };
            }
            catch (ApiException ex)
            {
                throw new ApiException(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                throw new ApiException(500, ex.Message);
            }
        }
    }
}
