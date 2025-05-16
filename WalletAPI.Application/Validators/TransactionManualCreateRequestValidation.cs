using FluentValidation;
using WalletAPI.Application.DTOs.Transfer.Request;

namespace WalletAPI.Application.Validators
{
    internal class TransactionManualCreateRequestValidation : AbstractValidator<TransactionManualCreateRequestDto>
    {
        public TransactionManualCreateRequestValidation()
        {
            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("O valor é obrigatório.")
                .GreaterThan(0).WithMessage("O valor deve ser maior que zero.");
            RuleFor(x => x.SenderEmailOrId)
                .NotEmpty()
                .WithMessage("O SenderEmailOrId é obrigatório.");
            RuleFor(x => x.ReceiverEmailOrId)
                .NotEmpty()
                .WithMessage("O ReceiverEmailOrId é obrigatório.");
        }
    }

}
