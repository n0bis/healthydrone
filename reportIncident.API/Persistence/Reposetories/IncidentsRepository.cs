using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using reportIncident.API.Domain.Models;
using reportIncident.API.Domain.Repositories;
using reportIncident.API.Persistence.Contexts;

namespace reportIncident.API.Persistence.Reposetories
{
    public class IncidentsRepository : BaseRepository, IIncidentsRepository
    {
        public IncidentsRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Incident>> ListAsync()
        {
            return await _context.Incidents.ToListAsync();
        }
    }
}
