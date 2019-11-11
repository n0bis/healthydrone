using System;
namespace utm_service.Models
{
    public class Track
    {
        public Coordinates location { get; set; }
        public string timestamp { get; set; }
        public double altitudeAGL { get; } = 3.5;
        public string source { get; } = "simulator";
    }
}
