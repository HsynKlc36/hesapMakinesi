using FormHelper;
using HesapMakinesi.Context;
using HesapMakinesi.Context.SeedData;
using HesapMakinesi.Extensions;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient.X.XDevAPI.Common;
using Serilog;
using Serilog.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages().AddRazorPagesOptions(options => {
    options.RootDirectory = "/Pages";//sayfalarýn bulunduðu kök dizini temsil eder
    options.Conventions.AddPageRoute("/Index", "Index/{id?}");
    options.Conventions.AddPageRoute("/Admin", "Admin/{id?}"); 
}).AddRazorRuntimeCompilation(); 

builder.Services.AddCookieServices(builder.Configuration);
builder.Services.AddDataAccessServices(builder.Configuration);

builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");//ajax post iþlemleri için


//serilog
#region serilog
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.ResponseHeaders.Add("MyResponseHeader");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;

});
Logger log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt")
    .WriteTo.MySQL(builder.Configuration.GetConnectionString("MyDb"), "Logs")
    .MinimumLevel.Information()
              .CreateLogger();
builder.Host.UseSerilog(log);
#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSerilogRequestLogging();// bu middleware den sonraki middleware'leri kapsayarak loglama yapar!
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseHttpLogging();//http log(kullanýcýya ait bilgileri taþýr)

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy();

app.MapRazorPages();
app.UseFormHelper();
await AdminSeedData.SeedAsync(app.Configuration);
app.Run();
