using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using reportIncident.API.Domain.Models;
using reportIncident.API.Domain.Services.Communication;

namespace reportIncident.API.Domain.Services
{
    public interface IIncidentsService
    {
        Task<IEnumerable<Incident>> ListAsync();
        Task<SaveIncidentResponse> SaveAsync(Incident incident);


    }
}
