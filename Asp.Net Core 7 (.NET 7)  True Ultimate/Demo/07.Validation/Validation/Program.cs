using Validation.CustomModelsBinders;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddControllers();  
builder.Services.AddControllers().AddXmlSerializerFormatters();// for using xml parameter(json is default)

builder.Services.AddControllers(options =>
{
    options.ModelBinderProviders.Insert(0, new PersonModelBinderProvider());
});

// If using IIS:
builder.Services.Configure<IISServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});


var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();   

app.Run();
