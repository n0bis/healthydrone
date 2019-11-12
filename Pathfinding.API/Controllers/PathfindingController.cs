using System;
using Microsoft.AspNetCore.Mvc;
using Pathfinding.API.Domain.Services;

namespace Pathfinding.API.Controllers
{
    [Route("api/Pathfinding")]
    [ApiController]
    public class PathfindingController : ControllerBase
    {
        private readonly IPathfindingService _pathfindingService;
        private readonly ICoordinateService _coordinateService;

        public PathfindingController(IPathfindingService pathfindingservice, ICoordinateService coordinateService)
        {
            _pathfindingService = pathfindingservice;
            _coordinateService = coordinateService;
        }

        // GET: api/Pathfinding
        [HttpGet]
        public string[] GetPath()
        {
            return _pathfindingService.FindPath();
        }
    }
}
