using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LandingPointsAPI.Domain.Models;
using LandingPointsAPI.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LandingPointsAPI.Controllers
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