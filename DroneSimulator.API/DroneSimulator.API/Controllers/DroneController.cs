using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DroneSimulator.API.Domain.Models;
using DroneSimulator.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DroneSimulator.API.Controllers
{
    [Route("api/[controller]")]
    public class DroneController : Controller
    {
        private DroneSim _droneSim;
        private readonly IConfiguration _config;

        public DroneController(IConfiguration config)
        {
            this._config = config;
            this._droneSim = new DroneSim(config);
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
