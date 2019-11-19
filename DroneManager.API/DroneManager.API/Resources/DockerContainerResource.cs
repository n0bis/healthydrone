using System;
namespace DroneManager.API.Resources
{
    public class DockerContainerResource
    {
        public string Id { get; set; }
        public int port { get; set; }
        public string droneId { get; set; }
    }
}
