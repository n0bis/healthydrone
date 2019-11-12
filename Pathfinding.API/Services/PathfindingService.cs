using Pathfinding.API.Domain.Models;
using Pathfinding.API.Domain.Services;
using RoyT.AStar;
using Newtonsoft.Json;

namespace Pathfinding.API.Services
{

    public class PathfindingService : IPathfindingService
    {

        public CoordinatesService _coordinateService = new CoordinatesService();

        public string[] FindPath()
        {
            AStarService astar = new AStarService();
            //TODO: Get start and end from UTM
            FlightPath fp = new FlightPath(55.059750, 10.606870, 55.385391, 10.366900);

            Position[] path = astar.pathfind(fp);
            return (PathConverter(path,fp));
        }

        public string[] PathConverter(Position[] path, FlightPath flightPath)
        {
            string[] strings = new string[path.Length + 2];
            strings[0] = JsonConvert.SerializeObject(flightPath.startCoordinate);
            int count = 1;
            foreach (Position pos in path)
            {
                strings[count] = JsonConvert.SerializeObject(_coordinateService.CalculateCoordinate(pos));
                count++;
            }
            strings[count] = JsonConvert.SerializeObject(flightPath.endCoordinate);
            return strings;
        }
    }
}