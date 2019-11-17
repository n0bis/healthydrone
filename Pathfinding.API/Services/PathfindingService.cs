using Pathfinding.API.Domain.Models;
using Pathfinding.API.Domain.Services;
using RoyT.AStar;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;

namespace Pathfinding.API.Services
{

    public class PathfindingService : IPathfindingService
    {

        public CoordinatesService _coordinateService = new CoordinatesService();

        public List<Coordinate> FindPath(Coordinate startPoint, Coordinate endPoint)
        {
            AStarService astar = new AStarService();
            //TODO: Get start and end from UTM
            FlightPath fp = new FlightPath(startPoint.latitude, startPoint.longitude, endPoint.latitude, endPoint.longitude);
            List<Position> paths = astar.pathfind(fp).ToList();

            List<Coordinate> coordinates = paths.Select(_coordinateService.CalculateCoordinate).ToList();

            return coordinates;
        }
    }
}