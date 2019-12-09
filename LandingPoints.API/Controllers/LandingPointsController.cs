using LandingPoints.API.Domain.LandingPointsAPI.Domain.Models;
using LandingPoints.API.Domain.Services;
using LandingPoints.API.Resources;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using LandingPoints.API.Extensions;
using AutoMapper;
using LandingPoints.API.Services;

namespace LandingPoints.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LandingPointsController : Controller
    {
        private readonly ILandingPointService _landingPointService;
        private readonly IMapper _mapper;

        public LandingPointsController(ILandingPointService landingPointService, IMapper mapper)
        {
            _landingPointService = landingPointService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<LandingPointResource>> GetAllAsync()
        {
            var landingPointsTemp = await _landingPointService.ListAsync();
            var landingpoints = _mapper.Map<IEnumerable<LandingPoint>, IEnumerable<LandingPointResource>>(landingPointsTemp);

            return landingpoints;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SaveLandingPointResource resource) 
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState.GetErrorMessages());

            var landingpoint = _mapper.Map<SaveLandingPointResource, LandingPoint>(resource);
            var result = await _landingPointService.SaveAsync(landingpoint);

            if (!result.Success)
                return BadRequest(result.Message);

            var landingPointResource = _mapper.Map<LandingPoint, LandingPointResource>(result.LandingPoint);
            return Ok(landingPointResource);
        }
    }
}
