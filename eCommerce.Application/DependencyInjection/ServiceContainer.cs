using eCommerce.Application.Mapping;
using eCommerce.Application.Services.Implementations;
using eCommerce.Application.Services.Implementations.Authentication;
using eCommerce.Application.Services.Implementations.Cart;
using eCommerce.Application.Services.Implementations.Identity;
using eCommerce.Application.Services.Implementations.Profile;
using eCommerce.Application.Services.Interfaces;
using eCommerce.Application.Services.Interfaces.Cart;
using eCommerce.Application.Services.Interfaces.Identity;
using eCommerce.Application.Services.Interfaces.Profile;
using eCommerce.Application.Validators.ValidationService.ValidationService;
using eCommerce.Application.Validators.Validator;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.Application.DependencyInjection
{
    //application services registration(mapping configuration, services)
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingConfig));
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IPaymentMethodService, PaymentMethodService>();
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateUserValidator>();
            services.AddValidatorsFromAssemblyContaining<LoginUserValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateCategoryValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateCategoryValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateProductValidator>();
            services.AddValidatorsFromAssemblyContaining<ChangePasswordValidator>();

            services.AddScoped<IValidationService, ValidationService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IGoogleAuthService, GoogleAuthService>();
            services.AddScoped<GoogleLoginHandler>();

            return services;
        }
    }
}
