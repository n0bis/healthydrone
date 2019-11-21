using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pathfinding.API.Domain.Models
{
    public class Coordinate
    {
        public double latitude { get; private set; }
        public double longitude { get; private set; }

        public Coordinate(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public override String ToString()
        {
            return (latitude + "," + longitude);
        }
    }
}
