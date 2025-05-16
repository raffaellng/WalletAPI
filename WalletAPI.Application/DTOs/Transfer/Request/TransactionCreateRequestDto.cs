using System.ComponentModel.DataAnnotations;

namespace WalletAPI.Application.DTOs.Transfer.Request
{
    public class TransactionCreateRequestDto
    {
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string ReceiverEmail { get; set; }
        [Required(ErrorMessage = "O campo Amount é obrigatório ")]
        public decimal Amount { get; set; }
    }
}
