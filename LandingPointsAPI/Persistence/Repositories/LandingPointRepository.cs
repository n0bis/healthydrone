using LandingPointsAPI.Domain.Models;
using LandingPointsAPI.Domain.Repositories;
using LandingPointsAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPointsAPI.Persistence.Repositories
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
    }
}
