using eCommerce.Application.DTOs.Category;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Application.Validators
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("Category Id is required.")
                .GreaterThan(0).WithMessage("Category Id must be greater than zero.");
        }
    }
}
