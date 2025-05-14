using WalletAPI.Domain.Entities;

namespace WalletAPI.Domain.Interfaces
{
    public interface IAuthService
    {
        string GenerateToken(string userId, string userEmail);
        Task<User?> GetUser(string email);
    }
}
