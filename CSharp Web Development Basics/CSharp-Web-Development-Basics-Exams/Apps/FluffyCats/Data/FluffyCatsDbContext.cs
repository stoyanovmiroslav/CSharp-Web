using FluffyCats.Models;
using Microsoft.EntityFrameworkCore;

namespace FluffyCats.Data
{
    public class FluffyCatsDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Kitten> Kittens { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=FluffyCats;Integrated Security=True;");
        }
    }
}
