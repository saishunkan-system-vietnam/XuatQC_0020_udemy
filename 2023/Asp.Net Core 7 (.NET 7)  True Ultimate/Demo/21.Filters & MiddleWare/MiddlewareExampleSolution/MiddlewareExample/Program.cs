using MiddlewareExample.CustomMiddleware;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<MyCustomMiddleware>();
var app = builder.Build();


// Middleware 1
app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync("Hello world\n");
    await next(context);
});

// Middleware 2
//app.Use(async (HttpContext context, RequestDelegate next) =>
//{
//    await context.Response.WriteAsync("Hello again");
//    await next(context);
//});

//app.UseMyCustomMiddleware();

// Middleware conventional với điều kiện 
// app.UseHelloWorldCustomMiddleware();

// run middle if condition is true
app.UseWhen(context => context.Request.Query.ContainsKey("userName"), app =>
{
    app.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("Hello world from middleware branch\n");
        await next(context);
    });
});


// Middleware 3
app.Run(async (HttpContext context) =>
{
    await context.Response.WriteAsync("More Hello again\n");
});
app.Run();
