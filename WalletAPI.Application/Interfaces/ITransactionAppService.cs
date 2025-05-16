using WalletAPI.Application.DTOs.Transfer.Request;
using WalletAPI.Application.DTOs.Transfer.Response;

namespace WalletAPI.Application.Interfaces
{
    public interface ITransactionAppService
    {
        Task CreateTransferAsync(Guid senderUserId, TransactionCreateRequestDto dto);
        Task CreateManualTransferAsync(TransactionManualCreateRequestDto dto);
        Task<IEnumerable<TransactionResponseDto>> ListUserTransactionsAsync(Guid sessionUserId, string? email, DateTime? inicio, DateTime? fim);
    }
}
