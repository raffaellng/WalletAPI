using FluentValidation;
using WalletAPI.Application.DTOs.Wallet.Request;

namespace WalletAPI.Application.Validators
{
    public class WalletDepositRequestValidator : AbstractValidator<WalletDepositRequestDto>
    {
        public WalletDepositRequestValidator()
        {
            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("O valor é obrigatório.")
                .GreaterThan(0).WithMessage("O valor deve ser maior que zero.");
        }
    }
}
