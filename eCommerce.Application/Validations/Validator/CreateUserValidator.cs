using eCommerce.Application.DTOs.User;
using eCommerce.Application.Validators.ValidationExtensions;
using FluentValidation;

namespace eCommerce.Application.Validators.Validator
{
    public class CreateUserValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserValidator()
        {
            RuleFor(u => u.Email)
                .ValidEmail();

            RuleFor(u => u.Password)
                .StrongPassword();

            RuleFor(u => u.FullName)
                .ValidName();

            RuleFor(u => u.ConfirmPassword)
                .Equal(u => u.Password).WithMessage("password don't match.");
        }
    }
}
