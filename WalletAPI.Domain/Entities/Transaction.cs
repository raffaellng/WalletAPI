namespace WalletAPI.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid SenderUserId { get; set; }
        public Guid ReceiverUserId { get; set; }
        public decimal Amount { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public User SenderUser { get; set; }
        public User ReceiverUser { get; set; }

        protected Transaction() { }

        public Transaction(Guid senderUserId, Guid receiverUserId, decimal amount)
        {
            if (senderUserId == receiverUserId)
                throw new ArgumentException("A transferência deve ser para outro usuário.");

            if (amount <= 0)
                throw new ArgumentException("O valor deve ser maior que zero.");

            SenderUserId = senderUserId;
            ReceiverUserId = receiverUserId;
            Amount = amount;
        }
    }
}