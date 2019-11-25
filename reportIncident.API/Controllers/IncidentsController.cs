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

        public IncidentsController(IIncidentsService incidentsService)
        {
            _incidentsService = incidentsService;
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

 

        // GET: api/Incidents/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Incidents
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Incidents/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
