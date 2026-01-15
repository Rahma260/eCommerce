using eCommerce.Application.DependencyInjection;
using eCommerce.Application.DTOs;
using eCommerce.Infrastructure.DependencyInjection;
using eCommerce.Infrastructure.Middleware;
using eCommerce.Presentation;
using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Serilog configuration
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    //create file for each day
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
builder.Host.UseSerilog(); // Use Serilog for logging
Log.Logger.Information("Application is starting...");

// Add services to the container.
builder.Services
    .AddControllers();

// Add Swagger/OpenAPI services
builder.Services.AddEndpointsApiExplorer(); // Required for minimal APIs
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ECommerce",
        Version = "v1"
    });

    // Define the JWT security scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey, // <-- change to ApiKey so we can control input
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Description = "Enter JWT token only (without 'Bearer ')."
    });

    // Add security requirement
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Optional: automatically add 'Bearer ' prefix to the header
    c.OperationFilter<SwaggerAddBearerPrefixOperationFilter>();
});


// For db configuration and repository services
builder.Services.AddInfrastructureService(builder.Configuration);
//add authentication in program.cs not infrastructure layer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // Try to read token directly from header without "Bearer "
            if (context.Request.Headers.TryGetValue("Authorization", out var token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        }
    };
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)
            ),
        ClockSkew = TimeSpan.Zero
    };
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    options.SaveTokens = true;

    options.Scope.Clear();
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");

    options.CallbackPath = "/api/Auth/google-callback";
    options.CorrelationCookie.SameSite = SameSiteMode.None;
    options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
});

//add cloudinary settings 
builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings"));

//add email settings
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(
        builder.Configuration.GetConnectionString("con"));
});

builder.Services.AddHangfireServer();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
    options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
    {
        EndPoints = { builder.Configuration["Redis:ConnectionString"] },
        ConnectTimeout = 10000, 
        SyncTimeout = 10000
    };
});

// For mapping configuration and application services
builder.Services.AddApplicationServices();


//add cors
builder.Services.AddCors(builder =>
{
    builder.AddDefaultPolicy(options =>
    {
        options.AllowAnyMethod()
               .AllowAnyHeader()
               .AllowAnyOrigin();
               //.AllowCredentials();
    });
});

try
{
    var app = builder.Build();

    // Enable CORS
    app.UseCors(); 

    // Log HTTP requests
    app.UseSerilogRequestLogging(); 

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger(); // enable middleware to serve generated Swagger JSON
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "eCommerce API V1");
            c.RoutePrefix = "swagger"; // Swagger UI at root /
        });
    }
    // Use global exception handling middleware
  //  app.UseInfrastructureService();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.UseHangfireDashboard("/hangfire", new DashboardOptions
    {
        DashboardTitle = "eCommerce Jobs Dashboard",
    });
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    //log
    Log.Logger.Information("Application is running.");
    app.Run();
}
catch (Exception ex)
{
    //log error
    Log.Logger.Error(ex, "Application failed to start correctly");
}
finally
{
    //log close
    Log.CloseAndFlush();
}
