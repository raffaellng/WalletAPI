using System.ComponentModel.DataAnnotations;

namespace WalletAPI.Application.DTOs.Wallet.Request
{
    public class WalletDepositRequestDto
    {
        [Required(ErrorMessage = "O Amount é obrigatório.")]
        public decimal Amount { get; set; }
    }
}
