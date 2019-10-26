using LandingPoints.API.Domain.LandingPointsAPI.Domain.Models;
using LandingPoints.API.Domain.Repositories;
using LandingPoints.API.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPoints.API.Persistance.Repositories
{
    public class LandingPointRepository : BaseRepository, ILandingPointRepository
    {
        public LandingPointRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<LandingPoint>> ListAsync()
        {
            return await _context.LandingPoints.ToListAsync();
        }

        public async Task AddAsync(LandingPoint landingPoint)
        {
            await _context.LandingPoints.AddAsync(landingPoint);
        }
    }
}
