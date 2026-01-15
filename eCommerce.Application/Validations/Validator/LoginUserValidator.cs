using eCommerce.Application.DTOs.User;
using eCommerce.Application.Validators.ValidationExtensions;
using FluentValidation;

namespace eCommerce.Application.Validators.Validator
{
    public class LoginUserValidator : AbstractValidator<LoginUserDto>
    {
        public LoginUserValidator()
        {
            RuleFor(u => u.Email)
                .ValidEmail();
            RuleFor(u => u.Password)
                .NotEmpty()
                .WithMessage("Password is required.");
        }
    }
}
