using System;
using System.Threading.Tasks;
using DroneManager.API.Domain.Models;
using DroneManager.API.Domain.Services.Communication;

namespace DroneManager.API.Domain.Services
{
    public interface IDroneService
    {
        Task<SaveDroneResponse> SaveAsync(Drone drone);
    }
}
