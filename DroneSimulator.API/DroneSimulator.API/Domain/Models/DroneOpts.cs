using System;
namespace DroneSimulator.API.Domain.Models
{
    public class DroneOpts
    {
        public string id { get; set; }
        public string operationid { get; set; }
        public double velocity { get; set; }
        public Location location { get; set; }
        public Location homelocation { get; set; }
    }
}
