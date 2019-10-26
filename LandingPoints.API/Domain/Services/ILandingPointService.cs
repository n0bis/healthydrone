using LandingPoints.API.Domain.LandingPointsAPI.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandingPoints.API.Domain.Services
{
    public interface ILandingPointService
    {
        Task<IEnumerable<LandingPoint>> ListAsync();
    }
}
