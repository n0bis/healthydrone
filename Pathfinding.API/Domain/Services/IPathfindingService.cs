using Pathfinding.API.Domain.Models;
using RoyT.AStar;

namespace Pathfinding.API.Domain.Services
{

    public interface IPathfindingService
    {
        public Coordinate[] FindPath();
        public Coordinate[] PathConverter(Position[] pos);
    }
}