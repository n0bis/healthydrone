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

        public LandingPointService(ILandingPointRepository landingPointRepository)
        {
            this._landingPointRepository = landingPointRepository;
        }

        public async Task<IEnumerable<LandingPoint>> ListAsync()
        {
            return await _landingPointRepository.ListAsync();
        }

        public Task<SaveLandingPointResponse> SaveAsync(LandingPoint landingPoint)
        {
            throw new NotImplementedException();
        }
    }
}
