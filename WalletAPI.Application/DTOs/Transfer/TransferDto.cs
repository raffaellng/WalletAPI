namespace WalletAPI.Application.DTOs.Transfer
{
    public class TransferDto
    {
        public Guid ReceiverUserId { get; set; }
        public decimal Amount { get; set; }
    }
}
