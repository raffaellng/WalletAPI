using FluentValidation;
using WalletAPI.Application.DTOs.User.Request;

namespace WalletAPI.Application.Validators
{
    public class UserCreateRequestValidator : AbstractValidator<UserCreateRequestDto>
    {
        public UserCreateRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("O nome é obrigatório.");
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("O e-mail é obrigatório.")
                .EmailAddress()
                .WithMessage("E-mail inválido.");
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("A senha é obrigatória.")
                .MinimumLength(6).WithMessage("A senha tem que ter pelo menos 6 caracteres.");
        }
    }
}
