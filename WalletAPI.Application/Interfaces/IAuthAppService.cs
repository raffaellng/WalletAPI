using WalletAPI.Application.DTOs.Auth.Request;
using WalletAPI.Application.DTOs.Auth.Response;

namespace WalletAPI.Application.Interfaces
{
    public interface IAuthAppService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }
}
