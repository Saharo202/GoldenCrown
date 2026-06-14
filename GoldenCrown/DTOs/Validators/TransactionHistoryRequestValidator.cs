using FluentValidation;
using GoldenCrown.DTOs.Finance;

namespace GoldenCrown.DTOs.Validators
{
    public class TransactionHistoryRequestValidator : AbstractValidator<TransactionHistoryRequest>
    {
        public TransactionHistoryRequestValidator() 
        {
            RuleFor(x => x.Limit)
                .GreaterThan(0).WithMessage("Значение limit должно быть больше 1");
            RuleFor(x => x.Offset)
                .GreaterThanOrEqualTo(0).WithMessage("Значение offset не может быть отрицательным");
        }
    }
}
