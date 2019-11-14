using LandingPoints.API.Domain;
using LandingPoints.API.Domain.LandingPointsAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPoints.API.Persistance.Contexts
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
                    callsign = "OUH-1",
                    latitude = 55.059750,
                    longitude = 10.606870,
                    type = EType.Hospital
                },
                new LandingPoint
                {
                    id = Guid.NewGuid(),
                    name = "Svendborg Sygehus (OUH)",
                    address = "Baagøes Alle 31, 5700 Svendborg",
                    description = "A hospital",
                    callsign = "OUH-2",
                    latitude = 55.385391,
                    longitude = 10.366900,
                    type = EType.Hospital
                }
            );



        }
    }
}
