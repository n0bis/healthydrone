using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reportIncident.API.Domain.Models;
using reportIncident.API.Domain.Services;
using reportIncident.API.Resources;
using AutoMapper;
using reportIncident.API.Extensions;

namespace reportIncident.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentsController : ControllerBase
    {

        private readonly IIncidentsService _incidentsService;
        private readonly IMapper _mapper;

        public IncidentsController(IIncidentsService incidentsService, IMapper mapper)
        {
            _incidentsService = incidentsService;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IEnumerable<IncidentResource>> GetAllAsync()
        {
            var incidents = await _incidentsService.ListAsync();
            var resources = _mapper.Map<IEnumerable<Incident>, IEnumerable<IncidentResource>>(incidents);
            return resources;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] SaveIncidentResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var incident = _mapper.Map<SaveIncidentResource, Incident>(resource);
            var result = await _incidentsService.SaveAsync(incident);

            if (!result.Succes)
                return BadRequest(result.Message);

            var incidentResource = _mapper.Map<Incident, IncidentResource>(result.Incident);
            return Ok(incidentResource);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> PutAsync(Guid Id, [FromBody] SaveIncidentResource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var incident = _mapper.Map<SaveIncidentResource, Incident>(resource);
            var result = await _incidentsService.UpdateAsync(Id, incident);

            if (!result.Succes)
                return BadRequest(result.Message);

            var incidentResource = _mapper.Map<Incident, IncidentResource>(result.Incident);
            return Ok(incidentResource);
        }
 

        

        
        
    }
}
