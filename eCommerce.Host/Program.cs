using eCommerce.Application.DependencyInjection;
using eCommerce.Application.Validators;
using eCommerce.Infrastructure.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

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
    //.AddFluentValidation(fv =>
    //{
    //    fv.RegisterValidatorsFromAssemblyContaining<CreateCategoryValidator>();
    //    fv.RegisterValidatorsFromAssemblyContaining<UpdateCategoryValidator>();
    //    fv.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>();
    //    fv.RegisterValidatorsFromAssemblyContaining<UpdateProductValidator>();
    //});

// Add Swagger/OpenAPI services
builder.Services.AddEndpointsApiExplorer(); // Required for minimal APIs
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "eCommerce API",
        Version = "v1",
        Description = "API for eCommerce application"
    });

});

// For db configuration and repository services
builder.Services.AddInfrastructureService(builder.Configuration);

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
    app.UseInfrastructureService();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

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
