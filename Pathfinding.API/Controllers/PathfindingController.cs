using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Pathfinding.API.Domain.Models;
using Pathfinding.API.Domain.Services;
using Pathfinding.API.Resources;

namespace Pathfinding.API.Controllers
{
    [Route("api/Pathfinding")]
    [ApiController]
    public class PathfindingController : ControllerBase
    {
        private readonly IPathfindingService _pathfindingService;

        public PathfindingController(IPathfindingService pathfindingservice)
        {
            _pathfindingService = pathfindingservice;
        }

        // GET: api/Pathfinding
        [HttpPost]
        public List<Coordinate> GetPath([FromBody] PathResource pathResource)
        {
            return _pathfindingService.FindPath(pathResource.StartPoint, pathResource.EndPoint);
        }
    }
}
