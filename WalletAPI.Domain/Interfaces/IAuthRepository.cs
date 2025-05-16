using WalletAPI.Domain.Entities;

namespace WalletAPI.Domain.Interfaces
{
    public interface IAuthRepository
    {
        string GenerateToken(string userId, string userEmail);
        Task<User?> GetUser(string email);
    }
}
