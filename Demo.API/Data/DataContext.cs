using Demo.API.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Demo.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }

        public DbSet<Value> Values { get; set; }
        public DbSet<User> Users { get; set; }
    }
}