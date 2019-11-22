﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reportIncident.API.Domain.Models;
using reportIncident.API.Domain.Services;
using reportIncident.API.Resources;

namespace reportIncident.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentsController : ControllerBase
    {

        private readonly IIncidentsService _incidentsService;

        public IncidentsController(IIncidentsService incidentsService)
        {
            _incidentsService = incidentsService;
        }


        [HttpGet]
        public async Task<IEnumerable<Incident>> GetAllAsync()
        {
            var incidents = await _incidentsService.ListAsync();
            return incidents;
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