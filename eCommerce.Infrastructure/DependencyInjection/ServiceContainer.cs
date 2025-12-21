using eCommerce.Domain.Interfaces;
using eCommerce.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EntityFramework.Exceptions.SqlServer;
using eCommerce.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using eCommerce.Infrastructure.Middleware;
using eCommerce.Application.Services.Interfaces.Logging;
using eCommerce.Application.Services.Implementations.Logging;


namespace eCommerce.Infrastructure.DependencyInjection
{
    //infrastructure srrvices registration(db context, repositories, exception handling middleware)
    public static class ServiceContainer
    {
        //IConfiguration configuration to get connection string from appsettings.json
        //this IServiceCollection services to make the method an extension method for IServiceCollection and allow us to call it in the Program.cs file 
        //y3ni k2nha b2t goz2 mn services msh method 3ady
        //(services.AddIfrastructureService) not (ServiceContainer.AddIfrastructureService(services, configuration))
        //extension method to add infrastructure services to the IServiceCollection(use database)
        public static IServiceCollection AddInfrastructureService(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var con = "con";
            services.AddDbContext<DBContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString(con),
                    b => b.MigrationsAssembly(typeof(DBContext).Assembly.FullName)
                    ).UseExceptionProcessor(),
                ServiceLifetime.Scoped
                );
            //register generic repository for dependency injection
            services.AddScoped<IGenericRepository<Product>, GenericRepository<Product>>();
            services.AddScoped<IGenericRepository<Category>, GenericRepository<Category>>();
            //register app logger service
            services.AddScoped(typeof(IAppLogger<>), typeof(SerilogLoggerAdapter<>));
            //register unit of work for dependency injection
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
        //extension method for IApplicationBuilder to use global exception handling middleware 
        public static IApplicationBuilder UseInfrastructureService(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            return app;
        }
    }
}
