using System;
using System.Collections.Generic;

namespace DroneSimulator.API.Domain.Models
{
    public class Drone
    {
        public double Velocity { get; set; }
        public string Id { get; set; }
        public string OperationId { get; set; }
        public Mission mission { get; set; } = null;
        public Location Location { get; set; }
        public Location HomeLocation { get; set; }
    }
}
