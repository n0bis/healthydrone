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

        public async Task AddAsync(Incident incident)
        {
            await _context.Incidents.AddAsync(incident);
        }

        public async Task<Incident> FindByIdAsync(Guid id)
        {
            return await _context.Incidents.FindAsync(id);
        }

        public void Update(Incident incident)
        {
            _context.Incidents.Update(incident);
        }

        public void Remove(Incident incident)
        {
            _context.Incidents.Remove(incident);
        }
    }
}
