using Pathfinding.API.Domain.Models;
using RoyT.AStar;

namespace Pathfinding.API.Domain.Services
{

    public interface IPathfindingService
    {
        public string[] FindPath();
        public string[] PathConverter(Position[] pos, FlightPath flightPath);
    }
}