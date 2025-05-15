using WalletAPI.Domain.Entities;

namespace WalletAPI.Domain.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetByUserAsync(Guid userId, DateTime? from = null, DateTime? to = null);
    }
}
