using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pathfinding.API.Domain.Models
{
    public class Coordinate
    {
        public double _latitude { get; private set; }
        public double _longitude { get; private set; }

        public Coordinate(double latitude, double longitude)
        {
            _latitude = latitude;
            _longitude = longitude;
        }

        public override String ToString()
        {
            return (_latitude + "," + _longitude);
        }
    }
}
