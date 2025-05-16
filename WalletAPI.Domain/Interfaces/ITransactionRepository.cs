using WalletAPI.Domain.Entities;

namespace WalletAPI.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction);
        Task<IEnumerable<Transaction>> ListByUserAsync(Guid senderUserId, DateTime? inicio, DateTime? fim);

    }
}
