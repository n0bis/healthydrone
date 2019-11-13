using RoyT.AStar;
using Pathfinding.API.Domain.Models;

namespace Pathfinding.API.Domain.Services
{
    public interface IAStarService
    {
        public Position[] pathfind(FlightPath flightpath);
        public void blockPath();
    }
}
