using System;
namespace utm_service.Models
{
    public class Track
    {
        public string timestamp { get; set; }
        public Coordinates location { get; set; }
        public Coordinates pilotLocation { get; set; }
        public double altitudeAGL { get; set; }
        public string source { get; set; }
    }
}
