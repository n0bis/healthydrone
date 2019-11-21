using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using reportIncident.API.Domain.Models;

namespace reportIncident.API.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Incidents> Incidents { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Incidents>().ToTable("Incidents");
            builder.Entity<Incidents>().HasKey(p => p.Id);
            builder.Entity<Incidents>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Incidents>().Property(p => p.Date).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Incidents>().Property(p => p.Drone).IsRequired();
            builder.Entity<Incidents>().Property(p => p.Operation).IsRequired();
            builder.Entity<Incidents>().Property(p => p.Details).IsRequired();
            builder.Entity<Incidents>().Property(p => p.Damage).IsRequired();
            builder.Entity<Incidents>().Property(p => p.Actions).IsRequired();
            builder.Entity<Incidents>().Property(p => p.Notes).IsRequired();
            builder.Entity<Incidents>().Property(p => p.File);
            
            builder.Entity<Incidents>().HasData

                (
                new Incidents { Id = 100, Date = DateTime.Now, Operation = "", Drone = "DJI", Details = "crashed", Damage = "2 propels down", Actions = "fetch drone from tree", Notes = "dont hit a tree", File = null }
                )
           
              
        }
    }
   
}