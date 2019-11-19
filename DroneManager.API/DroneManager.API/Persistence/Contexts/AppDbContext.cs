using System;
using DroneManager.API.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DroneManager.API.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<DockerContainer> DockerContainers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<DockerContainer>().ToTable("DockerContainers");
            builder.Entity<DockerContainer>().HasKey(p => p.Id);
            builder.Entity<DockerContainer>().Property(p => p.Id).IsRequired();
            builder.Entity<DockerContainer>().Property(p => p.port).IsRequired().HasMaxLength(5);
            builder.Entity<DockerContainer>().Property(p => p.droneId).IsRequired();
        }
    }
}
