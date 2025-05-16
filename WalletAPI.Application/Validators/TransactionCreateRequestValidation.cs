using FluentValidation;
using WalletAPI.Application.DTOs.Transfer.Request;

namespace WalletAPI.Application.Validators
{
    internal class TransactionCreateRequestValidation : AbstractValidator<TransactionCreateRequestDto>
    {
        public TransactionCreateRequestValidation()
        {
            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("O valor é obrigatório.")
                .GreaterThan(0).WithMessage("O valor deve ser maior que zero.");
            RuleFor(x => x.ReceiverEmail)
                .EmailAddress().WithMessage("Formato do email inválido.");
        }
    }
}
