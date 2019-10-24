using LandingPointsAPI.Domain.Models;
using LandingPointsAPI.Domain.Repositories;
using LandingPointsAPI.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPointsAPI.Services
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
    }
}
