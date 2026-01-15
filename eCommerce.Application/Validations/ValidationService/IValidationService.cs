using eCommerce.Application.DTOs.Responses;
using FluentValidation;

namespace eCommerce.Application.Validators.ValidationService.ValidationService
{
    public interface IValidationService
    {
        Task<ServiceResponse> ValidateAsync<T>(T model, IValidator<T> validator);
    }
}
