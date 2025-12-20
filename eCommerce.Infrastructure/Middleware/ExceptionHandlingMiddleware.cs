using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace eCommerce.Infrastructure.Middleware
{
    //global exception handling middleware for database update exceptions(best practice)
    //request delegate to pass the request to the next middleware in the pipeline
    public class ExceptionHandlingMiddleware(RequestDelegate _next)
    {
        //the logging will be in the file Logs and console using serilog
        //invoke method to handle the HTTP context
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                //pass the request
                await _next(context);
            }
            catch (DbUpdateException ex)
            {
                //get logger service from the request services
                var logger = context.RequestServices.GetRequiredService<ILogger<ExceptionHandlingMiddleware>>();
              
                //var innerException = ex.InnerException as SqlException; //explicit casting cause the exception type is SqlException
                //check for specific SQL error codes
                context.Response.ContentType = "application/json";
                if (ex.InnerException is SqlException innerException)
                {
                    //log error with serilog
                    logger.LogError(innerException, "SQL Exception");

                    switch(innerException.Number)
                    {
                        case 2627: // Violation of primary key constraint
                        case 2601: // Violation of unique index constraint
                            //409 Conflict the server cannot complete the request because it conflicts with the current state of the target resource
                            context.Response.StatusCode = StatusCodes.Status409Conflict;
                            await context.Response.WriteAsync("A conflict occurred due to duplicate data.");
                            break;
                        case 547: // Foreign key constraint violation
                            //400 Bad Request for foreign key constraint violations
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            await context.Response.WriteAsync("A foreign key constraint violation occurred.");
                            break;
                        case 1205: // Deadlock
                            //503 Service Unavailable for deadlock situations
                            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                            await context.Response.WriteAsync("The request could not be completed due to a database deadlock. Please try again.");
                            break;
                        case -2: // Timeout
                            //504 Gateway Timeout for timeout exceptions
                            context.Response.StatusCode = StatusCodes.Status504GatewayTimeout;
                            await context.Response.WriteAsync("The database operation timed out. Please try again later.");
                            break;
                        case 4060: // Database unavailable
                            //503 Service Unavailable for database unavailable exceptions
                            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                            await context.Response.WriteAsync("The database is currently unavailable. Please try again later.");
                            break;
                        case 515: // Null value violation
                            //400 Bad Request for null value violations
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            await context.Response.WriteAsync("Can't insert null.");
                            break;
                        default:
                            //500 Internal Server Error for other database update exceptions
                            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            await context.Response.WriteAsync("An error occurred while updating the database.");
                            break;
                    }
                }
                else
                {
                    //log error with serilog
                    logger.LogError(ex, "Related EFCore Exception");

                    //500 Internal Server Error for other database update exceptions
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync("An error occurred while updating the database.");
                }
            }
            catch (Exception ex)
            {
                //log error with serilog
                //repeat to get logger service from the request services (because it is out of scope)
                var logger = context.RequestServices.GetRequiredService<ILogger<ExceptionHandlingMiddleware>>();
                
                //log unknown exception
                logger.LogError(ex, "Unknown Exception");

                context.Response.ContentType = "application/json";
              
                //500 Internal Server Error for general exceptions
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
