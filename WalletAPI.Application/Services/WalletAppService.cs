using WalletAPI.Application.Interfaces;
using WalletAPI.Domain.Entities;
using WalletAPI.Domain.Exceptions;
using WalletAPI.Domain.Interfaces;
using WalletAPI.Domain.Utils;


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
                throw new ApiException( 401,Mensagens.ErroValorWallet);

            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new ApiException(404, Mensagens.UsuarioNaoEncontrado);

            user.Wallet.AddBalance(amount);
            await _userRepository.UpdateAsync(user);
        }

        public async Task AddBalanceAsync(string emailOrId, decimal amount)
        {
            User? targetUser = await ValidEmailOrId(emailOrId);

            targetUser.Wallet.AddBalance(amount);

            await _userRepository.UpdateAsync(targetUser);
        }


        public async Task<decimal> GetBalanceAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new ApiException(404, Mensagens.UsuarioNaoEncontrado);

            return user.Wallet.Balance;
        }

        public async Task<decimal> GetBalanceByEmailOrIdAsync(Guid userId, string emailOrId)
        {
            User? targetUser = await ValidEmailOrId(emailOrId);

            return targetUser.Wallet.Balance;
        }

        private async Task<User?> ValidEmailOrId(string emailOrId)
        {
            User? targetUser;

            if (Guid.TryParse(emailOrId, out var targetUserId))
                targetUser = await _userRepository.GetByIdAsync(targetUserId);
            else
                targetUser = await _userRepository.GetByEmailAsync(emailOrId);

            if (targetUser == null)
                throw new ApiException(404, Mensagens.UsuarioNaoEncontrado);
            return targetUser;
        }
    }
}