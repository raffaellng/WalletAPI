using System.ComponentModel.DataAnnotations;

namespace WalletAPI.Application.DTOs.Transfer.Request
{
    public class TransactionManualCreateRequestDto
    {
        [Required(ErrorMessage = "O campo senderEmailOrId é obrigatório.")]
        public string SenderEmailOrId { get; set; }

        [Required(ErrorMessage = "O campo receiverEmailOrId é obrigatório.")]
        public string ReceiverEmailOrId { get; set; }

        [Required(ErrorMessage = "O compo amount é obrigatório.")]
        public decimal Amount { get; set; }
    }
}
