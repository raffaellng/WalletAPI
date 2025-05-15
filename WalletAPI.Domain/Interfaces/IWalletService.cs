using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletAPI.Domain.Interfaces
{
    public interface IWalletService
    {
        Task<decimal> GetBalanceAsync(Guid userId);
        Task AddBalanceAsync(Guid userId, decimal amount);
        Task TransferAsync(Guid senderUserId, Guid receiverUserId, decimal amount);
    }
}
