using eCommerce.Application.DTOs.Responses;
using eCommerce.Application.Validators.ValidationService.ValidationService;
using FluentValidation;

namespace eCommerce.Application.Validators.ValidationService.ValidationService
{
    public class ValidationService : IValidationService
    {
        public async Task<ServiceResponse> ValidateAsync<T>(T model, IValidator<T> validator)
        {
            var validationResuult = await validator.ValidateAsync(model);
            if(!validationResuult.IsValid)
            {
                var errors = validationResuult.Errors.Select(e => e.ErrorMessage).ToList();
                string errorsToString = string.Join("; ", errors);
                return new ServiceResponse { message = errorsToString };
            }
            return new ServiceResponse { success = true };
        }
    }
}
;