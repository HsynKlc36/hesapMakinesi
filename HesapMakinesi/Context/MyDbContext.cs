using HesapMakinesi.Models;
using Microsoft.EntityFrameworkCore;

namespace HesapMakinesi.Context
{
    public class MyDbContext:DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }
        
        public DbSet<Mathematics> Mathematics { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
