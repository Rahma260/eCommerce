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
using eCommerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using eCommerce.Domain.Interfaces.Identity;
using eCommerce.Infrastructure.Repositories.Identity;


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
            //configure identity options
            services.AddDefaultIdentity<User>(
                options =>
                {
                    options.SignIn.RequireConfirmedEmail = true;
                    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
                    options.Password.RequireDigit = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireLowercase = true;
                    options.Password.RequiredUniqueChars = 1;
                })
                .AddRoles<Role>()
                .AddEntityFrameworkStores<DBContext>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
               // options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!))
                };
            });
            services.AddScoped<ITokenManagement, TokenManagement>();
            services.AddScoped<IRoleManagement, RoleManagement>();
            services.AddScoped<IUserManagement, UserManagement>();
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
