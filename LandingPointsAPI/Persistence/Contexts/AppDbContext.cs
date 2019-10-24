using LandingPointsAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPointsAPI.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<LandingPoint> LandingPoints { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<LandingPoint>().ToTable("Landing Points");
            builder.Entity<LandingPoint>().HasKey(p => p.id);
            builder.Entity<LandingPoint>().Property(p => p.id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<LandingPoint>().Property(p => p.latitude).IsRequired();
            builder.Entity<LandingPoint>().Property(p => p.longitude).IsRequired();
            builder.Entity<LandingPoint>().Property(p => p.callsign).IsRequired().HasMaxLength(10);
            builder.Entity<LandingPoint>().Property(p => p.description).IsRequired();
            builder.Entity<LandingPoint>().Property(p => p.name).IsRequired();
            builder.Entity<LandingPoint>().Property(p => p.address).IsRequired();
            builder.Entity<LandingPoint>().Property(p => p.type).IsRequired();


            _ = builder.Entity<LandingPoint>().HasData
            (
                new LandingPoint
                {
                    id = Guid.NewGuid(),
                    name = "Odense Universitets Hospital",
                    address = "J. B. Winsløws Vej 4, 5000 Odense",
                    description = "A hospital",
                    callsign = "OUH",
                    latitude = 55.385391,
                    longitude = 10.366900,
                    type = EType.Hospital
                }
            );

            

        }


    }
}
