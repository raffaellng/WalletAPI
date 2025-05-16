using WalletAPI.Application.DTOs.Transfer.Request;

namespace WalletAPI.Application.Interfaces
{
    public interface ITransactionAppService
    {
        Task CreateTransferAsync(Guid senderUserId, TransactionCreateRequestDto dto);
        Task CreateManualTransferAsync(TransactionManualCreateRequestDto dto);
    }
}
