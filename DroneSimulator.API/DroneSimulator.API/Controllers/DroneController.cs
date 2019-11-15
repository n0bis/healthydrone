using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using DroneSimulator.API.Domain.Models;
using DroneSimulator.API.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace DroneSimulator.API.Controllers
{
    [Route("api/[controller]")]
    public class DroneController : Controller
    {
        private readonly IDroneSim _droneSim;
        private static object _lock = new object();
        private static CancellationToken _cancellationToken;
        private static CancellationTokenSource _cancellationTokenSource;
        private static Task task;

        public DroneController(IDroneSim droneSim)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            this._droneSim = droneSim;
        }

        [HttpGet]
        [Route("/stop")]
        public IActionResult Stop()
        {
            CheckTask();

            task = Task.Run(async () => {
                await this._droneSim.Hover(_cancellationTokenSource.Token);
            }, _cancellationTokenSource.Token);

            return Ok();
        }

        [HttpGet]
        [Route("/sendHome")]
        public IActionResult SendHome()
        {
            CheckTask();
            
            task = Task.Run(async () => {
                await this._droneSim.SendHome(_cancellationTokenSource.Token);
            }, _cancellationTokenSource.Token);

            return Ok();
        }

        [HttpPost]
        [Route("/sendOnMission")]
        public IActionResult SendOnMission([FromBody] List<Location> locations)
        {
            CheckTask();

            task = Task.Run(async () => {
                if (_cancellationToken.IsCancellationRequested)
                    _cancellationToken.ThrowIfCancellationRequested();

                await this._droneSim.SendOnMission(locations, _cancellationTokenSource.Token);
            }, _cancellationTokenSource.Token);

            return Ok();
        }

        [HttpPost]
        [Route("/landAtLocation")]
        public IActionResult LandAtLocation([FromBody] Location location)
        {
            CheckTask();

            task = Task.Run(async () => {
                await this._droneSim.LandAtLocation(location, _cancellationTokenSource.Token);
            }, _cancellationTokenSource.Token);

            return Ok();
        }

        private void CheckTask()
        {
            if (task != null) {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Token.WaitHandle.WaitOne();
                //_cancellationTokenSource = new CancellationTokenSource();
            }
        }
    }
}
