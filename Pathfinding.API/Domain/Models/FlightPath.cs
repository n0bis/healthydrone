using System;
using Pathfinding.API.Domain.Models;

namespace Pathfinding.API.Domain.Models
{

    public class FlightPath
    {
        public Coordinate startCoordinate;
        public Coordinate endCoordinate;
        public Guid id { get; set; }

        public FlightPath(double startlat, double startlon, double endlat, double endlon)
        {
            startCoordinate = new Coordinate(startlat, startlon);
            endCoordinate = new Coordinate(endlat, endlon);
        }
    }
}