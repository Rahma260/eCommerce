using eCommerce.Application.DTOs.User;
using FluentValidation;

namespace eCommerce.Application.Validators
{
    public class LoginUserValidator : AbstractValidator<LoginUserDto>
    {
        public LoginUserValidator()
        {
            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.");
            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
