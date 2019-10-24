using LandingPointsAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPointsAPI.Domain.Services
{
    public interface ILandingPointService
    {
        Task<IEnumerable<LandingPoint>> ListAsync();
    }
}
