namespace CoursWork.Drive.Server.Middlewares
{
    public class AuthenticationHelperMiddleware 
    {
        private readonly RequestDelegate _next;
        public AuthenticationHelperMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context) 
        {
            return Task.CompletedTask;
        }
    }
}
