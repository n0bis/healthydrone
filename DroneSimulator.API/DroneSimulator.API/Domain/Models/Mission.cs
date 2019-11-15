using System;
using System.Collections.Generic;

namespace DroneSimulator.API.Domain.Models
{
    public class Mission
    {
        public TimeSpan TimeEstimated { get; set; }
        public List<Location> Waypoints { get; set; }
    }
}
