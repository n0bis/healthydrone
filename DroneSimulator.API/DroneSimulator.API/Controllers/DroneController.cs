using System.Collections.Generic;
using System.Threading.Tasks;
using DroneSimulator.API.Domain.Models;
using DroneSimulator.API.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace DroneSimulator.API.Controllers
{
    [Route("api/[controller]")]
    public class DroneController : Controller
    {
        private IDroneSim _droneSim;

        public DroneController(IDroneSim droneSim)
        {
            this._droneSim = droneSim;
        }

        [HttpGet]
        [Route("/sendHome")]
        public async Task SendHome()
        {
            await this._droneSim.SendHome();
        }

        [HttpPost]
        [Route("/sendOnMission")]
        public async Task SendOnMission([FromBody] List<Location> locations)
        {
            await this._droneSim.SendOnMission(locations);
        }

        [HttpPost]
        [Route("/landAtLocation")]
        public async Task LandAtLocation([FromBody] Location location)
        {
            await this._droneSim.LandAtLocation(location);
        }
    }
}
