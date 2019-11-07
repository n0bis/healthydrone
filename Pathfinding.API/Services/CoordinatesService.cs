using System;
using Pathfinding.API.Domain.Models;
using Pathfinding.API.Domain.Services;
using RoyT.AStar;

namespace Pathfinding.API.Services
{
    public class CoordinatesService : ICoordinateService
    {


        private int _x;
        private int _y;
        private double _lat;
        private double _lon;
        // These Numbers are used to calculate coordinates for Fyn only. For a detailed description check the Readme.
        private readonly double xStep = 618898 / 1000;
        private readonly double yStep = 1000000 / 1000;
        private readonly int xAdd = 55000000;
        private readonly int yAdd = 9900000;
        private readonly int div = 1000000;


        public Coordinate CalculateCoordinate(Position pos)
        {
            _x = pos.X;
            _y = pos.Y;

            double latitude = (((xStep * _x) + xAdd) / div);
            double longitude = (((yStep * _y) + yAdd) / div);

            return (new Coordinate(latitude, longitude));
        }

        public Position CalculatePosition(Coordinate coord)
        {
            _lat = coord._latitude;
            _lon = coord._longitude;

            int x = Convert.ToInt32(((_lat * div) - xAdd) / xStep);
            int y = Convert.ToInt32(((_lon * div) - yAdd) / yStep);

            return (new Position(x, y));
        }

        public String[] CoordinateToString(Coordinate[] coords)
        {
            String[] strings = new String[coords.Length];
            int count = 0;
            foreach (Coordinate c in coords)
            {
                strings[count] = c.ToString();
                count++;
            }
            return strings;
        }
    }
}