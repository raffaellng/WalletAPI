using WalletAPI.Domain.Entities;

namespace WalletAPI.Domain.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(string name, string email, string password);
    }
}
