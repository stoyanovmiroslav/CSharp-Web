using Microsoft.EntityFrameworkCore;
using SIS.CakeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.CakeApp.Data
{
    public class CakeAppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products{ get; set; }

        public DbSet<OrderProduct> OrderProducts { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server =.\\SQLEXPRESS; Database = Cake; Trusted_Connection = True; ");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderProduct>().HasKey(k => new { k.OrderId, k.ProductId });
        }
    }
}