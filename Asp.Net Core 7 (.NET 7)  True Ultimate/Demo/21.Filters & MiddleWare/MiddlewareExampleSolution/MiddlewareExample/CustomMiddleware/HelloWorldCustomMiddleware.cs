using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MiddlewareExample.CustomMiddleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class HelloWorldCustomMiddleware
    {
        private readonly RequestDelegate _next;

        public HelloWorldCustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Query.ContainsKey("firstName") &&
                httpContext.Request.Query.ContainsKey("lastName"))
            {
                string fullName = httpContext.Request.Query["firstName"] + " " + httpContext.Request.Query["lastName"];
                await httpContext.Response.WriteAsync(fullName);
            }

            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class HelloWorldCustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseHelloWorldCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HelloWorldCustomMiddleware>();
        }
    }
}
