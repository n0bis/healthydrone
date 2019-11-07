using Pathfinding.API.Domain.Models;
using Pathfinding.API.Domain.Services;
using RoyT.AStar;

namespace Pathfinding.API.Services
{

    public class PathfindingService : IPathfindingService
    {

        public CoordinatesService _coordinateService = new CoordinatesService();

        public Coordinate[] FindPath()
        {
            AStarService astar = new AStarService();
            //TODO: Get start and end from UTM
            Position[] path = astar.pathfind(new FlightPath(55.059750, 10.606870, 55.385391, 10.366900));
            return (PathConverter(path));
        }

        public Coordinate[] PathConverter(Position[] path)
        {
            Coordinate[] coords = new Coordinate[path.Length];
            int count = 0;
            foreach (Position pos in path)
            {
                coords[count] = _coordinateService.CalculateCoordinate(pos);
                count++;
            }
            return coords;
        }
    }
}