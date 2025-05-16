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

        public Transaction(Guid senderId, Guid receiverId, decimal amount)
        {
            if (senderId == receiverId)
                throw new ArgumentException("Usuário não pode transferir para si mesmo.");

            if (amount <= 0)
                throw new ArgumentException("Valor da transferência deve ser maior que zero.");

            SenderUserId = senderId;
            ReceiverUserId = receiverId;
            Amount = amount;
        }
    }
}