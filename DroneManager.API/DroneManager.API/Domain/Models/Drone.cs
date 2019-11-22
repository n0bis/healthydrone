using System;
namespace DroneManager.API.Domain.Models
{
    public class Drone
    {
        public string Id { get; set; }
        public string OperationId { get; set; }
        public double velocity { get; set; }
        public Location CurrentLocation { get; set; }
        public Location HomeLocation { get; set; }
    }
}
