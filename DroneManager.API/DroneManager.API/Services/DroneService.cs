using System;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using DroneManager.API.Domain.Models;
using DroneManager.API.Domain.Services;
using DroneManager.API.Domain.Services.Communication;

namespace DroneManager.API.Services
{
    public class DroneService : IDroneService
    {
        private readonly DockerClient _dockerClient;

        public DroneService()
        {
            _dockerClient = new DockerClientConfiguration(new Uri(""))
                .CreateClient();
        }

        public async Task SpinUpContainer()
        {
            var container = await _dockerClient.Containers.CreateContainerAsync("");
            await _dockerClient.Containers.StartContainerAsync(
                    container.getId(),
                    new HostConfig
                    {
                        PortBindings = 
                    });
        }

        public Task<SaveDroneResponse> SaveAsync(Drone drone)
        {
            throw new NotImplementedException();
        }
    }
}
