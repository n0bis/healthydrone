using LandingPoints.API.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPoints.API.Resources
{
    public class LandingPointResource
    {
        public Double latitude { get; set; }
        public Double longitude { get; set; }
        public String callsign { get; set; }
        public String description { get; set; }
        public String name { get; set; }
        public String address { get; set; }
        public EType type { get; set; }
        public Guid id { get; set; }
    }
}
