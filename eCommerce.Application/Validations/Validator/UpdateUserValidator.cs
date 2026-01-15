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
    public class UpdateUserValidator : AbstractValidator<UserDto>
    {
        public UpdateUserValidator()
        {
            RuleFor(u => u.Email)
                .ValidEmail();

            RuleFor(u => u.FullName)
                .ValidName();
        }
    }
}
