using System;
namespace utm_service.Models
{
    public class UAS
    {
        public Guid uniqueIdentifier { get; set; }
        public string model { get; set; }
        public string series { get; set; }
        public string nickname { get; set; }
        public double mtom { get; set; }
        public string flightStatus { get; set; }
    }
}
