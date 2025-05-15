using WalletAPI.Application.Interfaces;
using WalletAPI.Domain.Entities;
using WalletAPI.Domain.Interfaces;

namespace WalletAPI.Application.Services
{
    public class WalletAppService : IWalletAppService
    {
        private readonly IWalletService _walletService;
        private readonly ITransactionService _transactionService;

        public async Task<decimal> GetBalanceAsync(Guid userId)
        {
            return await _walletService.GetBalanceAsync(userId);
        }

        public async Task AddBalanceAsync(Guid userId, decimal amount)
        {
            await _walletService.AddBalanceAsync(userId, amount);
        }

        public async Task TransferAsync(Guid senderUserId, Guid receiverUserId, decimal amount)
        {
            await _walletService.TransferAsync(senderUserId, receiverUserId, amount);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsAsync(Guid userId, DateTime? from = null, DateTime? to = null)
        {
            return await _transactionService.GetByUserAsync(userId, from, to);
        }
    }
}