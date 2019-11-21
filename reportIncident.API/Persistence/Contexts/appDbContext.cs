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

        protected override void onModelCreating(ModelBuilder builder)
        {
    
        }
    }

}   