using eCommerce.Application.DTOs.Product;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Application.Validators
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty().WithMessage("Product Id is required.")
                .GreaterThan(0).WithMessage("Product Id must be greater than zero.");

        }
    }

}
