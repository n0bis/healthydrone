using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DroneManager.API.Domain.Models;
using DroneManager.API.Domain.Services.Communication;

namespace DroneManager.API.Domain.Services
{
    public interface IDroneService
    {
        Task<IEnumerable<DockerContainer>> ListAsync();
        Task<SaveDockerResponse> CreateAndStartContainer(Drone drone);
        Task SpinUpContainer(string id);
        Task<SaveDockerResponse> SaveAsync(DockerContainer dockerContainer);
    }
}
