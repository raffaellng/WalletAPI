using WalletAPI.Application.Interfaces;
using WalletAPI.Domain.Entities;
using WalletAPI.Domain.Interfaces;

namespace WalletAPI.Application.Services
{
    public class WalletAppService : IWalletAppService
    {
        private readonly IUserRepository _userRepository;

        public WalletAppService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task AddBalanceAsync(Guid userId, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Valor deve ser maior que zero.");

            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new InvalidOperationException("Usuário não encontrado.");

            user.Wallet.AddBalance(amount);
            await _userRepository.UpdateAsync(user);
        }

        public async Task<decimal> GetBalanceAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new InvalidOperationException("Usuário não encontrado.");

            return user.Wallet.Balance;
        }


    }
}