using FluentValidation;
using FluentValidation.AspNetCore;
using FormHelper;
using HesapMakinesi.Context;
using HesapMakinesi.FluentFilter;
using HesapMakinesi.FluentValidatiors.AuthValidators;
using HesapMakinesi.Models.Enums;
using HesapMakinesi.VM;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Reflection;

namespace HesapMakinesi.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCookieServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                //IsPersistent özelliği true olarak ayarlanırsa, kullanıcının oturumu tarayıcıyı kapatsa bile devam eder
                options.SlidingExpiration = false;//oturum süresi tarayıcı kapatıldığında sonlanmaz ve oturum kapatılma süresi ertelenmez
                options.Cookie.Name = "HesapMakinesi";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(2);
                options.AccessDeniedPath = "/Auth/AccessDenied";/* new PathString("/Login/AccessDeniedPage")*/
                options.LoginPath = "/Auth/Login"; // Kimlik doğrulama başarısız olduğunda yönlendirme yapılacak sayfa
                options.Cookie.HttpOnly = true;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole(Role.Admin.ToString()));
                options.AddPolicy("RequireGuestRole", policy => policy.RequireRole(Role.Guest.ToString()));
                options.AddPolicy("RequireAdminOrGuestRole", policy => policy.RequireRole(Role.Admin.ToString(), Role.Guest.ToString()));
            });
            services.AddAutoMapper(
               Assembly.GetExecutingAssembly(),
               typeof(RegisterVM).Assembly);


            services.AddFluentValidationAutoValidation()
              .AddFluentValidationClientsideAdapters()//?
              .AddValidatorsFromAssemblyContaining<AuthRegisterVMValidator>();
            ValidatorOptions.Global.LanguageManager.Culture=new CultureInfo("tr");

            //validation filter(razor page)
            services.AddRazorPages()
            .AddMvcOptions(options =>
            {
                 options.Filters.Add<ValidationFilter>();
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes=true;//default Required attribute hatasından kurtarır! 
        });


            services.AddRazorPages().AddFormHelper();//form helper kullanabilmek için

            return services;
        }
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {

            string _getConnStringName = configuration.GetConnectionString("MyDb");

            services.AddDbContext<MyDbContext>(options =>
            {

                options.UseMySql(_getConnStringName, ServerVersion.AutoDetect(_getConnStringName));

            });


            return services;
        }
    }
}
