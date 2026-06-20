using FluentValidation;
using GoldenCrown.DTOs.Finance;

namespace GoldenCrown.DTOs.Validators
{
    public class TransferRequestValidator : AbstractValidator<TransferRequest>
    {    
            public TransferRequestValidator()
            {
                RuleFor(x => x.Amount)
                    .GreaterThan(0).WithMessage("Сумма должна быть больше 0");
                RuleFor(x => x.ReceiverLogin)
                    .NotEmpty().WithMessage("Сумма должна быть больше 0");
            }

    }


}

