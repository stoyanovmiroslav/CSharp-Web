using CakeApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CakeApp.Data
{
    public class CakeAppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products{ get; set; }

        public DbSet<OrderProduct> OrderProducts { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server =.\SQLEXPRESS; Database = Cake; Trusted_Connection = True; ");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderProduct>().HasKey(k => new { k.OrderId, k.ProductId });
        }
    }
}