using Pathfinding.API.Domain.Models;
using Pathfinding.API.Domain.Services;
using RoyT.AStar;

namespace Pathfinding.API.Services
{

    public class AStarService : IAStarService
    {

        CoordinatesService _coordinatesService = new CoordinatesService();
        Grid grid = new Grid(1000, 1000, 1.0f);

        public Position[] pathfind(FlightPath flightpath)
        {

            Position startPos = _coordinatesService.CalculatePosition(flightpath.startCoordinate);
            Position endPos = _coordinatesService.CalculatePosition(flightpath.endCoordinate);

            Position[] path = grid.GetPath(startPos, endPos);

            return path;
        }

        public void blockPath()
        {
            //TODO: get no-fly zones and block the path.
        }
    }
}