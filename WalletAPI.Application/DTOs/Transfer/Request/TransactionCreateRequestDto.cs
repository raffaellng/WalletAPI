namespace WalletAPI.Application.DTOs.Transfer.Request
{
    public class TransactionCreateRequestDto
    {
        public string ReceiverEmail { get; set; }
        public decimal Amount { get; set; }
    }
}
