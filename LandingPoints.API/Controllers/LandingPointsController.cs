using LandingPoints.API.Domain.LandingPointsAPI.Domain.Models;
using LandingPoints.API.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPoints.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LandingPointsController : Controller
    {
        private readonly ILandingPointService _landingPointService;

        public LandingPointsController(ILandingPointService landingPointService)
        {
            _landingPointService = landingPointService;
        }

        [HttpGet]
        public async Task<IEnumerable<LandingPoint>> GetAllAsync()
        {
            var landingpoints = await _landingPointService.ListAsync();
            return landingpoints;
        }
    }
}
