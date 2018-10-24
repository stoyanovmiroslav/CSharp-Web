using Microsoft.EntityFrameworkCore;
using MishMash.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MishMash.Data
{
    public class MishMashDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Channel> Channels { get; set; }

        public DbSet<UserChanel> UserChanels { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<ChanelTag> ChanelTags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=MishMashApp;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserChanel>().HasKey(x => new { x.ChannelId, x.UserId });
            modelBuilder.Entity<ChanelTag>().HasKey(x => new { x.ChannelId, x.TagId });
        }
    }
}