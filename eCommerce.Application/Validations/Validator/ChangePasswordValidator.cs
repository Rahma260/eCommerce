using eCommerce.Application.DTOs.User;
using eCommerce.Application.Validators.ValidationExtensions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Application.Validators.Validator
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordValidator()
        {
            RuleFor(p => p.Email)
                .ValidEmail();

            RuleFor(p => p.NewPassword)
                .StrongPassword();

            RuleFor(p => p.NewPassword)
                .Equal(p => p.CurrentPassword)
                .WithMessage("the new password is the same as current passsword, choose another one");
        }
    }
}
