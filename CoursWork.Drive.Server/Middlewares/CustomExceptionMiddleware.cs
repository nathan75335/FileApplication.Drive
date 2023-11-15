using CoursWork.Drive.BusinessLogic.Exceptions;
using System.Text;

namespace CoursWork.Drive.Server.Middlewares;

public class CustomExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException ex)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(ex.Message));
        }
        catch(ExistException ex)
        {
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(ex.Message));
        }
        catch(Exception ex)
        {
            context.Response.StatusCode= StatusCodes.Status500InternalServerError;
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(ex.Message));
        }
    }
}
