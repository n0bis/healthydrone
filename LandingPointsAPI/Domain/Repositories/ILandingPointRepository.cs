using LandingPointsAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPointsAPI.Domain.Repositories
{
    public interface ILandingPointRepository
    {
        Task<IEnumerable<LandingPoint>> ListAsync();
    }
}
