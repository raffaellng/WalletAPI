using System.ComponentModel.DataAnnotations;

namespace WalletAPI.Application.DTOs.Wallet.Request
{
    public class WalletDepositForIdOrEmailRequestDto
    {
        [Required(ErrorMessage = "O campo Amount é obrigatório.")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "O campo EmailOrId é obrigatório.")]
        public string EmailOrId { get; set; }
    }
}
