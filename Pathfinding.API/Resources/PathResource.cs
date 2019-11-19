using System;
using Pathfinding.API.Domain.Models;

namespace Pathfinding.API.Resources
{
    public class PathResource
    {
        public Coordinate StartPoint { get; set; }
        public Coordinate EndPoint { get; set; }
    }
}
