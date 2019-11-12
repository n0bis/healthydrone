using Pathfinding.API.Domain.Models;
using RoyT.AStar;
using Pathfinding.API.Domain.Models;

namespace Pathfinding.API.Domain.Services
{

    public interface IPathfindingService
    {
        public string[] FindPath();
        public string[] PathConverter(Position[] pos, FlightPath flightPath);
    }
}