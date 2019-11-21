using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using reportIncident.API.Domain.Models;
using reportIncident.API.Domain.Services;
using reportIncident.API.Domain.Repositories;

namespace reportIncident.API.Services
{
    public class IncidentsService : IIncidentsService
    {

        private readonly IIncidentsRepository _incidentsRepository;

        public IncidentsService(IIncidentsRepository incidentsRepository)
        {
            this._incidentsRepository = incidentsRepository;
        }
        public async Task<IEnumerable<Incidents>> ListAsync()
        {
            return await _incidentsRepository.ListAsync();
        }
    }
}
