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

        public async Task<IncidentResponse> SaveAsync(Incident incident)
        {
            try
            {
                await _incidentsRepository.AddAsync(incident);
                await _unitOfWork.CompleteAsync();

                return new IncidentResponse(incident);
            }
            catch (Exception ex)
            {
                return new IncidentResponse($"daaaaaaaaaaaaaamn : {ex.Message}");
            }
        }

        public async Task<IncidentResponse> UpdateAsync(Guid id, Incident incident)
        {
            var existingIncident = await _incidentsRepository.FindByIdAsync(id);

            if (existingIncident == null)
                return new IncidentResponse("Doesnt exist");

            existingIncident.Date = incident.Date;
            existingIncident.Actions = incident.Actions;
            existingIncident.Damage = incident.Damage;
            existingIncident.Details = incident.Details;
            existingIncident.Notes = incident.Notes;
            
            try
            {
                _incidentsRepository.Update(existingIncident);
                await _unitOfWork.CompleteAsync();

                return new IncidentResponse(existingIncident);
            }
            catch(Exception ex)
            {
                return new IncidentResponse($"cant update : {ex.Message}");
            }
        }

        public async Task<IncidentResponse> DeleteAsync(Guid id)
        {
            var existingIncident = await _incidentsRepository.FindByIdAsync(id);

            if (existingIncident == null)
                return new IncidentResponse("incident not found");

            try
            {
                _incidentsRepository.Remove(existingIncident);
                await _unitOfWork.CompleteAsync();

                return new IncidentResponse(existingIncident);
            }
            catch(Exception ex)
            {
                return new IncidentResponse($"An error occurred when deleting the category: {ex.Message}");
            }
        }
    }
}
