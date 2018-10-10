using IRunes.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Data
{
    public class IRunesDbContext : DbContext
    {
        public DbSet<Album> Albums { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Track> Tracks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=IRunesApp;Trusted_Connection=True;");
        }
    }
}