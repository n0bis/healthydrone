using System;
using DroneManager.API.Domain.Repositories;
using DroneManager.API.Persistence.Contexts;

namespace DroneManager.API.Persistence.Repositories
{
    public class DroneRepository : BaseRepository, IDroneRepository
    {
        public DroneRepository(AppDbContext context) : base(context)
        {
        }
    }
}
