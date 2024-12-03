using Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ICountriesService, CountriesService>();
builder.Services.AddTransient<IPersonService, PersonService>();

builder.Services.AddDbContext<PersonDBContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});


builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider sevices,
    LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
    .ReadFrom.Configuration(context.Configuration) // read configuration settings form built-in Iconfiguration
    .ReadFrom.Services(sevices); // read out current app's services and make them avilable to serilog
});

var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

app.UseHttpLogging();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
