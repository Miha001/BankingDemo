using BankingDemo.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BankingDemo.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {  
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex); 
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var problem = new ProblemDetails { Instance = context.Request.Path };

        switch (exception)
        {

            case ValidationException vex:
                problem.Status = StatusCodes.Status400BadRequest;
                problem.Title = "Validation Error";
                problem.Extensions["errors"] = vex.Errors.GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
                break;

            case InternalException bex:
                problem.Status = (int)bex.HttpStatusCode;
                problem.Title = bex.Message;
                break;

            default:
                logger.LogError(exception, "Internal Error");
                problem.Status = StatusCodes.Status500InternalServerError;
                problem.Title = "Server Error";
                break;
        }

        context.Response.StatusCode = problem.Status.Value;
        context.Response.ContentType = "application/problem+json";
        return context.Response.WriteAsJsonAsync(problem);
    }
}