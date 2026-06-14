using FluentValidation;
using GoldenCrown.DTOs.Users;

namespace GoldenCrown.DTOs.Validators
{
    public class RegisterRequesrValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequesrValidator()
        {
            RuleFor(x => x.Login)
                .NotEmpty().WithMessage("Поле login обязательно")
                .MinimumLength(3).WithMessage("Минимальная длина логина от 3 символов");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Поле password обязательно")
                .MinimumLength(6).WithMessage("Минимальная длина пароля 6 символов");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Поле name обязательно");

        }
    }
}
