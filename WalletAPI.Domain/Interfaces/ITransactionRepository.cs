using WalletAPI.Domain.Entities;

namespace WalletAPI.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction);
    }
}
