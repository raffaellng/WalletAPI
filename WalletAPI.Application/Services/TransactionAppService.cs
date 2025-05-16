using WalletAPI.Application.DTOs.Transfer.Request;
using WalletAPI.Application.DTOs.Transfer.Response;
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

        public async Task CreateManualTransferAsync(TransactionManualCreateRequestDto dto)
        {
            User? sender;
            if (Guid.TryParse(dto.SenderEmailOrId, out var senderId))
                sender = await _userRepository.GetByIdAsync(senderId);
            else
                sender = await _userRepository.GetByEmailAsync(dto.SenderEmailOrId);

            if (sender == null)
                throw new InvalidOperationException("Remetente não encontrado.");

            // Buscar destinatário
            User? receiver;
            if (Guid.TryParse(dto.ReceiverEmailOrId, out var receiverId))
                receiver = await _userRepository.GetByIdAsync(receiverId);
            else
                receiver = await _userRepository.GetByEmailAsync(dto.ReceiverEmailOrId);

            if (receiver == null)
                throw new InvalidOperationException("Destinatário não encontrado.");

            if (receiver.Id == sender.Id)
                throw new InvalidOperationException("Não é possível transferir para si mesmo.");

            sender.Wallet.SubtractBalance(dto.Amount);
            receiver.Wallet.AddBalance(dto.Amount);

            var transaction = new Transaction(sender.Id, receiver.Id, dto.Amount);

            await _userRepository.UpdateAsync(sender);
            await _userRepository.UpdateAsync(receiver);
            await _transactionRepository.AddAsync(transaction);
        }

        public async Task<IEnumerable<TransactionResponseDto>> ListUserTransactionsAsync(Guid sessionUserId, string? email, DateTime? inicio, DateTime? fim)
        {
            Guid userIdToSearch;

            if (!string.IsNullOrWhiteSpace(email))
            {
                var user = await _userRepository.GetByEmailAsync(email)
                    ?? throw new InvalidOperationException("Usuário não encontrado.");
                userIdToSearch = user.Id;
            }
            else
            {
                userIdToSearch = sessionUserId;
            }

            var transactions = await _transactionRepository.ListByUserAsync(userIdToSearch, inicio, fim);

            return transactions.Select(t => new TransactionResponseDto
            {
                Id = t.Id,
                Amount = t.Amount,
                CreatedAt = t.CreatedAt,
                ReceiverEmail = t.ReceiverUser?.Email ?? "Desconhecido"
            });
        }
    }
}