namespace WalletAPI.Application.DTOs.Transfer.Response
{
    public class TransactionResponseDto
    {
        public Guid Id { get; set; }
        public string ReceiverEmail { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
