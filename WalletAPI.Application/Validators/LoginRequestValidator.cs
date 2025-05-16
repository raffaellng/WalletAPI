using FluentValidation;
using WalletAPI.Application.DTOs.Auth.Request;

namespace WalletAPI.Application.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage("Formato do email inválido.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("A senha é obrigatória.")
                .MinimumLength(6).WithMessage("A senha tem que ter pelo menos 6 caracteres.");
        }
    }
}
