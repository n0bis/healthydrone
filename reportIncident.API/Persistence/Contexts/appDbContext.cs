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
        public DbSet<Incident> Incidents { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Incident>().ToTable("Incidents");
            builder.Entity<Incident>().HasKey(p => p.Id);
            builder.Entity<Incident>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Incident>().Property(p => p.Date).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Incident>().Property(p => p.Drone).IsRequired();
            builder.Entity<Incident>().Property(p => p.Operation).IsRequired();
            builder.Entity<Incident>().Property(p => p.Details).IsRequired();
            builder.Entity<Incident>().Property(p => p.Damage).IsRequired();
            builder.Entity<Incident>().Property(p => p.Actions).IsRequired();
            builder.Entity<Incident>().Property(p => p.Notes).IsRequired();

            _ = builder.Entity<Incident>().HasData

                (
                new Incident { Id = 100, Date = DateTime.Now, Operation = "", Drone = "DJI", Details = "crashed", Damage = "2 propels down", Actions = "fetch drone from tree", Notes = "dont hit a tree" }
                );
           
              
        }
    }
   
}