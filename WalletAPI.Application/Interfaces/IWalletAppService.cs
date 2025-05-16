using WalletAPI.Domain.Entities;

namespace WalletAPI.Application.Interfaces
{
    public interface IWalletAppService
    {
        Task<decimal> GetBalanceAsync(Guid userId);
        Task AddBalanceAsync(Guid userId, decimal amount);
        Task AddBalanceAsync(string emailOrId, decimal amount);
        Task<decimal> GetBalanceByEmailOrIdAsync(Guid userId, string emailOrId);
    }
}
