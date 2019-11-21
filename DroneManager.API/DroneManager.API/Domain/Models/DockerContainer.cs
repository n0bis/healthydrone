using System;
namespace DroneManager.API.Domain.Models
{
    public class DockerContainer
    {
        public string Id { get; set; }
        public int port { get; set; }
        public string droneId { get; set; }
    }
}
