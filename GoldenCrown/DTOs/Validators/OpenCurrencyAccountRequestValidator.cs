using FluentValidation;
using GoldenCrown.DTOs.Finance;

namespace GoldenCrown.DTOs.Validators
{
    public class OpenCurrencyAccountRequestValidator
     : AbstractValidator<OpenCurrencyAccountRequest>
    {
        public OpenCurrencyAccountRequestValidator()
        {
            RuleFor(x => x.Currency)
                .IsInEnum();
        }
    }
}
