using HesapMakinesi.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;
using System.Security.Cryptography;

namespace HesapMakinesi.Context.SeedData
{
    public static class AdminSeedData
    {
        private const string AdminEmail = "admin@hesapMakinesi.com";
        private const string AdminPassword = "Admin123";
        public static async Task SeedAsync(IConfiguration configuration)
        {
            var dbContextBuilder = new DbContextOptionsBuilder<MyDbContext>();
            string _getConnStringName = configuration.GetConnectionString("MyDb");

            dbContextBuilder.UseMySql(_getConnStringName, ServerVersion.AutoDetect(_getConnStringName));

            using MyDbContext context = new(dbContextBuilder.Options);

            if (!context.Users.Any(user => user.Email == AdminEmail))
            {
                await AddAdmin(context);
            }
            await Task.CompletedTask;

        }
        private static async Task AddAdmin(MyDbContext context)
        {
            User user = new User()
            {
                UserName = "Admin",
                Email = AdminEmail,
                Role = Models.Enums.Role.Admin,
                CreatedDate = DateTime.Now,
                IsActive = true


            };
            user.Password = await HashPasswordAsync(AdminPassword);
            await context.AddAsync(user);
            await context.SaveChangesAsync();
        }
        private static async Task<string> HashPasswordAsync(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
            return hashedPassword;
        }

    }
}
