using WalletAPI.Application.DTOs.User.Request;
using WalletAPI.Application.DTOs.User.Response;
using WalletAPI.Application.Interfaces;
using WalletAPI.Domain.Entities;
using WalletAPI.Domain.Interfaces;

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
            var existing = await _userRepository.GetByEmailAsync(dto.Email);
            if (existing != null)
                throw new InvalidOperationException("Email já está em uso.");

            var user = new User(dto.Name, dto.Email, BCrypt.Net.BCrypt.HashPassword(dto.Password));
            var created = await _userRepository.AddAsync(user);

            return new UserResponseDto
            {
                Id = created.Id,
                Name = created.Name,
                Email = created.Email
            };
        }
    }
}
