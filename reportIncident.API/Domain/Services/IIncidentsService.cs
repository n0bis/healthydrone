using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using reportIncident.API.Domain.Models;

namespace reportIncident.API.Domain.Services
{
    public interface IIncidentsService
    {
        Task<IEnumerable<Incident>> ListAsync();
    }
}
