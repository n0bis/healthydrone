using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DroneManager.API.Domain.Models;
using DroneManager.API.Domain.Repositories;
using DroneManager.API.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DroneManager.API.Persistence.Repositories
{
    public class DroneRepository : BaseRepository, IDroneRepository
    {
        public DroneRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<DockerContainer> FindByDroneIdAsync(string droneId)
        {
            return await _context.DockerContainers.FirstOrDefaultAsync(container => container.droneId == droneId);
        }

        public async Task<IEnumerable<DockerContainer>> ListAsync()
        {
            return await _context.DockerContainers.ToListAsync();
        }

        public async Task AddAsync(DockerContainer dockerContainer)
        {
            await _context.DockerContainers.AddAsync(dockerContainer);
        }
    }
}
