using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using reportIncident.API.Domain.Models;
using reportIncident.API.Domain.Services;
using reportIncident.API.Domain.Repositories;
using reportIncident.API.Domain.Services.Communication;

namespace reportIncident.API.Services
{
    public class IncidentsService : IIncidentsService
    {

        private readonly IIncidentsRepository _incidentsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public IncidentsService(IIncidentsRepository incidentsRepository, IUnitOfWork unitOfWork)
        {
            _incidentsRepository = incidentsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Incident>> ListAsync()
        {
            return await _incidentsRepository.ListAsync();
        }

        public async Task<SaveIncidentResponse> SaveAsync(Incident incident)
        {
            try
            {
                await _incidentsRepository.AddAsync(incident);
                await _unitOfWork.CompleteAsync();

                return new SaveIncidentResponse(incident);
            }
            catch (Exception ex)
            {
                return new SaveIncidentResponse($"daaaaaaaaaaaaaamn : {ex.Message}");
            }
        }

        
        
    }
}
