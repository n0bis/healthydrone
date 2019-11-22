using Pathfinding.API.Domain.Models;
using RoyT.AStar;

namespace Pathfinding.API.Domain.Services
{

    public interface ICoordinateService
    {
        public Coordinate CalculateCoordinate(Position pos);
        public Position CalculatePosition(Coordinate coord);
        public string[] CoordinateToString(Coordinate[] coords);
    }
}