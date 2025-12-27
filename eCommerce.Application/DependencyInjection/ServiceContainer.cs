using AutoMapper;
using eCommerce.Application.Mapping;
using eCommerce.Application.Services.Implementations;
using eCommerce.Application.Services.Implementations.Identity;
using eCommerce.Application.Services.Interfaces;
using eCommerce.Application.Services.Interfaces.Identity;
using eCommerce.Application.Validators;
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
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();
            services.AddValidatorsFromAssemblyContaining<LoginUserValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateCategoryValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateCategoryValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateProductValidator>();
            services.AddScoped<IValidationService, ValidationService>();
            services.AddScoped<IIdentityService, IdentityService>();
            return services;
        }
    }
}
