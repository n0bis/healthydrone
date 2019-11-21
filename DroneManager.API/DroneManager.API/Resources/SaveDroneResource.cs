using System;
using DroneManager.API.Domain.Models;

namespace DroneManager.API.Resources
{
    public class SaveDroneResource
    {
        public string Id { get; set; }
        public string OperationId { get; set; }
        public double velocity { get; set; }
        public Location CurrentLocation { get; set; }
        public Location HomeLocation { get; set; }
    }
}
