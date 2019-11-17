using System.Collections.Generic;
using Pathfinding.API.Domain.Models;
using RoyT.AStar;

namespace Pathfinding.API.Domain.Services
{

    public interface IPathfindingService
    {
        public List<Coordinate> FindPath(Coordinate startPoint, Coordinate endPoint);
    }
}