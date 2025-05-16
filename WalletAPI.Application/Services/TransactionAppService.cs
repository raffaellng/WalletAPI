using WalletAPI.Application.DTOs.Transfer.Request;
using WalletAPI.Application.Interfaces;
using WalletAPI.Domain.Entities;
using WalletAPI.Domain.Interfaces;

namespace WalletAPI.Application.Services
{
    public class TransactionAppService : ITransactionAppService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionAppService(
            IUserRepository userRepository,
            ITransactionRepository transactionRepository)
        {
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task CreateTransferAsync(Guid senderUserId, TransactionCreateRequestDto dto)
        {
            var sender = await _userRepository.GetByIdAsync(senderUserId)
                ?? throw new InvalidOperationException("Remetente não encontrado.");

            var receiver = await _userRepository.GetByEmailAsync(dto.ReceiverEmail)
                ?? throw new InvalidOperationException("Destinatário não encontrado.");

            if (receiver.Id == sender.Id)
                throw new InvalidOperationException("Não é possível transferir para si mesmo.");

            sender.Wallet.SubtractBalance(dto.Amount);
            receiver.Wallet.AddBalance(dto.Amount);

            var transaction = new Transaction(sender.Id, receiver.Id, dto.Amount);

            await _userRepository.UpdateAsync(sender);
            await _userRepository.UpdateAsync(receiver);
            await _transactionRepository.AddAsync(transaction);
        }
    }
}