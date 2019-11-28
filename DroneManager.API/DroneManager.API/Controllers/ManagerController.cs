using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DroneManager.API.Domain.Models;
using DroneManager.API.Domain.Services;
using DroneManager.API.Extensions;
using DroneManager.API.Resources;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DroneManager.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ManagerController : ControllerBase
    {
        private readonly IDroneService _droneService;
        private readonly IMapper _mapper;

        public ManagerController(IDroneService droneService, IMapper mapper)
        {
            _droneService = droneService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<DockerContainer>> Get()
        {
            return await _droneService.ListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SaveDroneResource resource)
        {
            var drone = _mapper.Map<SaveDroneResource, Drone>(resource);
            var result = await _droneService.CreateAndStartContainer(drone);

            if (!result.Success)
                return BadRequest(new ErrorResource(result.Message));

            var dockerContainerResource = _mapper.Map<DockerContainer, DockerContainerResource>(result.Resource);
            return Ok(dockerContainerResource);
        }
    }
}
