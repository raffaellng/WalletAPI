using WalletAPI.Domain.Entities;

namespace WalletAPI.Application.Interfaces
{
    public interface IWalletAppService
    {
        Task<decimal> GetBalanceAsync(Guid userId);
        Task AddBalanceAsync(Guid userId, decimal amount);
        Task TransferAsync(Guid senderUserId, Guid receiverUserId, decimal amount);
        Task<IEnumerable<Transaction>> GetTransactionsAsync(Guid userId, DateTime? from = null, DateTime? to = null);
    }
}
