using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DroneSimulator.API.Domain.Models;

namespace DroneSimulator.API.Domain.Services
{
    public interface IDroneSim
    {
        Task SendOnMission(List<Location> locations, CancellationToken cancellationToken);
        Task SendHome(CancellationToken cancellationToken);
        Task LandAtLocation(Location location, CancellationToken cancellationToken);
        Task Hover(CancellationToken cancellationToken);
    }
}
