using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DroneManager.API.Domain.Models;

namespace DroneManager.API.Domain.Repositories
{
    public interface IDroneRepository
    {
        Task<DockerContainer> FindByDroneIdAsync(string droneId);
        Task<IEnumerable<DockerContainer>> ListAsync();
        Task AddAsync(DockerContainer dockerContainer);
    }
}
