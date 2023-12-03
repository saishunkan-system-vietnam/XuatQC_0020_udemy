using CRUDExample.Entities;
using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<ICountriesService, CountriesService>();
builder.Services.AddSingleton<IPersonService, PersonService>();
builder.Services.AddSingleton<IFinnhubService, FinnhubService>();
builder.Services.AddSingleton<IStocksService, StocksService>();

builder.Services.AddOptions<TradingOptions>()
            .Bind(builder.Configuration.GetSection(nameof(TradingOptions)))
            .ValidateDataAnnotations();

var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
