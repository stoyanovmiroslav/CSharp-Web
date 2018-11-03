using Microsoft.EntityFrameworkCore;
using Airport.Models;

namespace Airport.Data
{
    public class AirportDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Fligh> Flighs { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=Airport;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}