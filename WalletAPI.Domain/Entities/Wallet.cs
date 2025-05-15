namespace WalletAPI.Domain.Entities
{
    public class Wallet
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public decimal Balance { get; private set; } = 0;
        public Guid UserId { get; set; }
        public User User { get; set; }

        public void AddBalance(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("O valor deve ser maior que zero.");
            Balance += amount;
        }

        public void SubtractBalance(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("O valor deve ser maior que zero.");

            if (Balance < amount)
                throw new InvalidOperationException("Saldo insuficiente.");
            Balance -= amount;
        }
    }
}