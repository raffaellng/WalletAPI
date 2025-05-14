using WalletAPI.Application.DTOs.Auth;

namespace WalletAPI.Application.Interfaces
{
    public interface IAuthAppService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }
}
