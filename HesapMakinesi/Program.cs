using FormHelper;
using HesapMakinesi.Context;
using HesapMakinesi.Context.SeedData;
using HesapMakinesi.Extensions;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages().AddRazorPagesOptions(options => {
    options.RootDirectory = "/Pages";//sayfalarýn bulunduðu kök dizini temsil eder
    options.Conventions.AddPageRoute("/Index", "Index/{id?}");
    options.Conventions.AddPageRoute("/Admin", "Admin/{id?}");
    //options.Conventions.Add(new SuppressImplicitRequiredAttributeConvention());

}).AddRazorRuntimeCompilation(); 
builder.Services.AddCookieServices(builder.Configuration);
builder.Services.AddDataAccessServices(builder.Configuration);
builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");//ajax post iþlemleri için
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy();

app.MapRazorPages();
app.UseFormHelper();
await AdminSeedData.SeedAsync(app.Configuration);
app.Run();
