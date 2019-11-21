using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using DroneManager.API.Configuration;
using DroneManager.API.Domain.Models;
using DroneManager.API.Domain.Repositories;
using DroneManager.API.Domain.Services;
using DroneManager.API.Domain.Services.Communication;
using Microsoft.Extensions.Options;

namespace DroneManager.API.Services
{
    public class DroneService : IDroneService
    {
        private readonly DockerClient _dockerClient;
        private readonly IOptions<UTMOpts> _utmOpts;
        private readonly IDroneRepository _droneRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DroneService(IOptions<UTMOpts> utmOptions, IDroneRepository droneRepository, IUnitOfWork unitOfWork)
        {
            _dockerClient = new DockerClientConfiguration(DockerApiUri())
                .CreateClient();
            _utmOpts = utmOptions;
            _droneRepository = droneRepository;
            _unitOfWork = unitOfWork;
        }

        private Uri DockerApiUri()
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            var isMac = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

            if (isWindows)
            {
                return new Uri("npipe://./pipe/docker_engine");
            }

            if (isLinux || isMac)
            {
                return new Uri("unix:/var/run/docker.sock");
            }

            throw new Exception("Was unable to determine what OS this is running");
        }

        public async Task<IEnumerable<DockerContainer>> ListAsync()
        {
            return await _droneRepository.ListAsync();
        }

        public async Task<SaveDockerResponse> CreateAndStartContainer(Drone drone)
        {
            if (await _droneRepository.FindByDroneIdAsync(drone.Id) != null)
                return new SaveDockerResponse($"Drone: {drone.Id} already exsits");

            var port = GetAvailablePort();
            var container = await _dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters {
                Image = "dronesimulator:latest",
                ExposedPorts = new Dictionary<string, EmptyStruct>
                {
                    { "80", default }
                },
                HostConfig = new HostConfig
                {
                    PortBindings = new Dictionary<string, IList<PortBinding>>
                    {
                        { "80", new List<PortBinding> { new PortBinding { HostPort = port.ToString() } } }
                    },
                    PublishAllPorts = true
                },
                Env = new List<string> {
                    "ENVIRONMENT=Development",
                    $"Drone__id={drone.Id}",
                    $"Drone__operationid={drone.OperationId}",
                    $"Drone__velocity={drone.velocity}",
                    $"Drone__location__latitude={drone.CurrentLocation.latitude}",
                    $"Drone__location__longitude={drone.CurrentLocation.longitude}",
                    $"Drone__homelocation__latitude={drone.HomeLocation.latitude}",
                    $"Drone__homelocation__longitude={drone.HomeLocation.longitude}",
                    $"UTM__clientid={_utmOpts.Value.clientid}",
                    $"UTM__clientsecret={_utmOpts.Value.clientsecret}",
                    $"UTM__username={_utmOpts.Value.username}",
                    $"UTM__password={_utmOpts.Value.password}"
                }
            });
            await _dockerClient.Containers.StartContainerAsync(container.ID, null);
            var dockerContainer = new DockerContainer { Id = container.ID, port = port, droneId = drone.Id };
            var response = await SaveAsync(dockerContainer);
            return response;
        }

        public async Task SpinUpContainer(string id)
        {
            await _dockerClient.Containers.StartContainerAsync(id, null);
        }

        private int GetAvailablePort()
        {
            var defaultLoopbackEndpoint = new IPEndPoint(IPAddress.Loopback, port: 0);
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Bind(defaultLoopbackEndpoint);
                return ((IPEndPoint)socket.LocalEndPoint).Port;
            }
        }

        public async Task<SaveDockerResponse> SaveAsync(DockerContainer dockerContainer)
        {
            try
            {
                await _droneRepository.AddAsync(dockerContainer);
                await _unitOfWork.CompleteAsync();

                return new SaveDockerResponse(dockerContainer);
            }
            catch (Exception ex)
            {
                return new SaveDockerResponse($"An error occurred when saving the docker container: {ex.Message}");
            }
        }
    }
}
