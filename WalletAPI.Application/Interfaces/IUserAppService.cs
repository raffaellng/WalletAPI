using WalletAPI.Application.DTOs.User.Request;
using WalletAPI.Application.DTOs.User.Response;

namespace WalletAPI.Application.Interfaces
{
    public interface IUserAppService
    {
        Task<UserResponseDto> CreateUserAsync(UserCreateRequestDto dto);
    }
}
