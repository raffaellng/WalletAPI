using WalletAPI.Application.DTOs.User.Request;
using WalletAPI.Application.DTOs.User.Response;
using WalletAPI.Application.Interfaces;
using WalletAPI.Domain.Entities;
using WalletAPI.Domain.Exceptions;
using WalletAPI.Domain.Interfaces;
using WalletAPI.Domain.Utils;

namespace WalletAPI.Application.Services
{
    public class UserAppService : IUserAppService
    {
        private readonly IUserRepository _userRepository;

        public UserAppService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponseDto> CreateUserAsync(UserCreateRequestDto dto)
        {
            try
            {
                var existing = await _userRepository.GetByEmailAsync(dto.Email);
                if (existing != null)
                    throw new ApiException(404, Mensagens.ErroEmail);

                var user = new User(dto.Name, dto.Email, BCrypt.Net.BCrypt.HashPassword(dto.Password));
                var created = await _userRepository.AddAsync(user);

                return new UserResponseDto
                {
                    Id = created.Id,
                    Name = created.Name,
                    Email = created.Email
                };
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
