namespace WalletAPI.Domain.Entities
{
    public class Wallet
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }

        public required User User { get; set; }
        public required ICollection<Transaction> TransactionsAsSender { get; set; }
        public required ICollection<Transaction> TransactionsAsReceiver { get; set; }
    }
}