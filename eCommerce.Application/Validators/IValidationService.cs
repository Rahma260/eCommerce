using eCommerce.Application.DTOs;
using FluentValidation;

namespace eCommerce.Application.Validators
{
    public interface IValidationService
    {
        Task<ServiceResponse> ValidateAsync<T>(T model, IValidator<T> validator);
    }
}
