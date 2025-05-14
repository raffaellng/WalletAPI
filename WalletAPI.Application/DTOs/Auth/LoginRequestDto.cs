using System.ComponentModel.DataAnnotations;

namespace WalletAPI.Application.DTOs.Auth
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Password { get; set; } = null!;
    }
}
