using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using DroneManager.API.Configuration;
using DroneManager.API.Domain.Models;
using DroneManager.API.Domain.Services;
using DroneManager.API.Domain.Services.Communication;
using Microsoft.Extensions.Options;

namespace DroneManager.API.Services
{
    public class DroneService : IDroneService
    {
        private readonly DockerClient _dockerClient;
        private readonly IOptions<UTMOpts> _utmOpts;

        public DroneService(IOptions<UTMOpts> utmOptions)
        {
            _dockerClient = new DockerClientConfiguration(new Uri(DockerApiUri()))
                .CreateClient();
            _utmOpts = utmOptions;
        }

        private string DockerApiUri()
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

            if (isWindows)
            {
                return "npipe://./pipe/docker_engine";
            }

            if (isLinux)
            {
                return "unix:/var/run/docker.sock";
            }

            throw new Exception("Was unable to determine what OS this is running");
        }

        public async Task SpinUpContainer(Drone drone)
        {
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
                        { "80", new List<PortBinding> { new PortBinding { HostPort = GetAvailablePort().ToString() } } }
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

        public Task<SaveDroneResponse> SaveAsync(Drone drone)
        {
            throw new NotImplementedException();
        }
    }
}
