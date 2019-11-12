using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DroneSimulator.API.Domain.Models;

namespace DroneSimulator.API.Domain.Services
{
    public interface IDroneSim
    {
        Task SendOnMission(List<Location> locations);
        Task SendHome();
        Task LandAtLocation(Location location);
    }
}
