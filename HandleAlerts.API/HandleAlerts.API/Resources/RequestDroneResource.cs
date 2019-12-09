using System;
namespace HandleAlerts.API.Resources
{
    public class RequestDroneResource
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Location toLocation { get; set; }
        public Location fromLocation { get; set; }
    }

    public class Location
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}
