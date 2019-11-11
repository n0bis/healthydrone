using System;
namespace DroneSimulator.API.Domain.Models
{
    public class Drone
    {
        public double Velocity { get; set; }
        public string Id { get; set; }
        public string OperationId { get; set; }
        public Location Location { get; set; }
        public Location HomeLocation { get; set; }
    }
}
