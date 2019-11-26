using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using reportIncident.API.Domain.Models;

namespace reportIncident.API.Domain.Repositories
{
    public interface IIncidentsRepository
    {
        Task<IEnumerable<Incident>> ListAsync();
        Task AddAsync(Incident incident);
        Task<Incident> FindByIdAsync(Guid id);
        void Update(Incident incident);
        void Remove(Incident incident);
    }
}
