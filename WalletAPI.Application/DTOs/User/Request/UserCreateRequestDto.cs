using System.ComponentModel.DataAnnotations;

namespace WalletAPI.Application.DTOs.User.Request
{
    public class UserCreateRequestDto
    {
        [Required(ErrorMessage = "O name é obrigatório.")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O password é obrigatório.")]
        public string Password { get; set; }
    }
}
