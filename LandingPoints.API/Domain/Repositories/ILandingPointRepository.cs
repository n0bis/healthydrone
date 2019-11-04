using LandingPoints.API.Domain.LandingPointsAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPoints.API.Domain.Repositories
{
    public interface ILandingPointRepository
    {
        Task<IEnumerable<LandingPoint>> ListAsync();
        Task AddAsync(LandingPoint landingPoint);
    }
}
