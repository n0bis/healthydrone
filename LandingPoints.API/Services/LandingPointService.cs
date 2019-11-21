using LandingPoints.API.Domain.LandingPointsAPI.Domain.Models;
using LandingPoints.API.Domain.Repositories;
using LandingPoints.API.Domain.Services;
using LandingPoints.API.Domain.Services.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPoints.API.Services
{
    public class LandingPointService : ILandingPointService
    {
        private readonly ILandingPointRepository _landingPointRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LandingPointService(ILandingPointRepository landingPointRepository, IUnitOfWork unitOfWork)
        {
            _landingPointRepository = landingPointRepository;
            _unitOfWork = unitOfWork;

        }

        public async Task<IEnumerable<LandingPoint>> ListAsync()
        {
            return await _landingPointRepository.ListAsync();
        }

        public async Task<SaveLandingPointResponse> SaveAsync(LandingPoint landingPoint)
        {
            try
            {
                await _landingPointRepository.AddAsync(landingPoint);
                await _unitOfWork.CompleteAsync();

                return new SaveLandingPointResponse(landingPoint);
            }
            catch (Exception ex)
            {
                return new SaveLandingPointResponse($"An error has occured while saving the Landing Point: {ex.Message}");
            }


        }
    }
}
