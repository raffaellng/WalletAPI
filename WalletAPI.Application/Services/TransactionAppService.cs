using WalletAPI.Application.DTOs.Transfer.Request;
using WalletAPI.Application.DTOs.Transfer.Response;
using WalletAPI.Application.Interfaces;
using WalletAPI.Domain.Entities;
using WalletAPI.Domain.Exceptions;
using WalletAPI.Domain.Interfaces;
using WalletAPI.Domain.Utils;

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
            try
            {
                var sender = await _userRepository.GetByIdAsync(senderUserId)
                    ?? throw new ApiException(404, Mensagens.RemetenteNaoEncontrado);

                var receiver = await _userRepository.GetByEmailAsync(dto.ReceiverEmail)
                    ?? throw new ApiException(404, Mensagens.DestinatarioNaoEncontrado);

                if (receiver.Id == sender.Id)
                    throw new ApiException(401, Mensagens.TransferenciaInvalida);

                sender.Wallet.SubtractBalance(dto.Amount);
                receiver.Wallet.AddBalance(dto.Amount);

                var transaction = new Transaction(sender.Id, receiver.Id, dto.Amount);

                await _userRepository.UpdateAsync(sender);
                await _userRepository.UpdateAsync(receiver);
                await _transactionRepository.AddAsync(transaction);
            }
            catch (ApiException ex)
            {
                throw new ApiException(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                throw new ApiException(500, ex.Message);
            }
        }

        public async Task CreateManualTransferAsync(TransactionManualCreateRequestDto dto)
        {
            try
            {
                User? sender;
                if (Guid.TryParse(dto.SenderEmailOrId, out var senderId))
                    sender = await _userRepository.GetByIdAsync(senderId);
                else
                    sender = await _userRepository.GetByEmailAsync(dto.SenderEmailOrId);

                if (sender == null)
                    throw new ApiException(404, Mensagens.RemetenteNaoEncontrado);

                // Buscar destinatário
                User? receiver;
                if (Guid.TryParse(dto.ReceiverEmailOrId, out var receiverId))
                    receiver = await _userRepository.GetByIdAsync(receiverId);
                else
                    receiver = await _userRepository.GetByEmailAsync(dto.ReceiverEmailOrId);

                if (receiver == null)
                    throw new ApiException(404, Mensagens.DestinatarioNaoEncontrado);

                if (receiver.Id == sender.Id)
                    throw new ApiException(401, Mensagens.TransferenciaInvalida);

                sender.Wallet.SubtractBalance(dto.Amount);
                receiver.Wallet.AddBalance(dto.Amount);

                var transaction = new Transaction(sender.Id, receiver.Id, dto.Amount);

                await _userRepository.UpdateAsync(sender);
                await _userRepository.UpdateAsync(receiver);
                await _transactionRepository.AddAsync(transaction);
            }
            catch (ApiException ex)
            {
                throw new ApiException(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                throw new ApiException(500, ex.Message);
            }
        }

        public async Task<IEnumerable<TransactionResponseDto>> ListUserTransactionsAsync(Guid sessionUserId, string? email, DateTime? inicio, DateTime? fim)
        {
            try
            {
                Guid userIdToSearch;

                if (!string.IsNullOrWhiteSpace(email))
                {
                    var user = await _userRepository.GetByEmailAsync(email)
                        ?? throw new ApiException(404, Mensagens.UsuarioNaoEncontrado);
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
            catch (ApiException ex)
            {
                throw new ApiException(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                throw new ApiException(500, ex.Message);
            }
        }
    }
}