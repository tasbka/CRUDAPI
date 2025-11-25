using BussinessLogic.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Infrastructure;

public class ExceptionFilter : IAsyncExceptionFilter
{
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        var exception = context.Exception;
        if (exception is not CrudApiExceptionBase)
        {
            return;
        }
        context.ExceptionHandled = true;

        switch (exception)
        {
            case CrudApiNotFoundExceptionBase ex:
                await WriteResponse(context.HttpContext, ex, 404);
                break;
        }
    }

    private static async Task WriteResponse(HttpContext httpContext, CrudApiExceptionBase exception, int statusCode)
    {
        httpContext.Response.StatusCode = statusCode;
        var response = new ResponseObject
        {
            Message = exception.Message,
            StatusCode = statusCode
        };
        await httpContext.Response.WriteAsJsonAsync(response);
    }
}