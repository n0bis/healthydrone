using LandingPoints.API.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPoints.API.Resources
{
    public class SaveLandingPointResource
    {
        [Required]
        public Double latitude { get; set; }
        [Required]
        public Double longitude { get; set; }
        [Required]
        public String callsign { get; set; }
        [Required]
        public String description { get; set; }
        [Required]
        public String name { get; set; }
        [Required]
        public String address { get; set; }
        [Required]
        public EType type { get; set; }
        [Required]
        public Guid id { get; set; }
    }
}
