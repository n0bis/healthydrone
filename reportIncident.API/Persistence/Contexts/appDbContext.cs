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

            builder.Entity<Incident>().ToTable("incidents");
            builder.Entity<Incident>().HasKey(p => p.Id);
            builder.Entity<Incident>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Incident>().Property(p => p.Date).IsRequired();
            builder.Entity<Incident>().Property(p => p.DroneId).IsRequired();
            builder.Entity<Incident>().Property(p => p.OperationId).IsRequired();
            builder.Entity<Incident>().Property(p => p.Details).IsRequired();
            builder.Entity<Incident>().Property(p => p.Damage).IsRequired();
            builder.Entity<Incident>().Property(p => p.Actions).IsRequired();
            builder.Entity<Incident>().Property(p => p.Notes).IsRequired();

            _ = builder.Entity<Incident>().HasData

                (
                new Incident { Id = Guid.NewGuid(), Date = DateTime.Now, OperationId = Guid.NewGuid(), DroneId = Guid.NewGuid(), Details = "crashed", Damage = "2 propels down", Actions = "fetch drone from tree", Notes = "dont hit a tree" }
                );
           
              
        }
    }
   
}