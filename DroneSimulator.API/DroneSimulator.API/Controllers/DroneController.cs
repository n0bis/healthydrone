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

        public DroneController(IDroneSim droneSim)
        {
            this._droneSim = droneSim;
        }

        [HttpGet]
        [Route("/stop")]
        public IActionResult Stop()
        {
            CheckTask();

            var task = Task.Run(async () => {
                await this._droneSim.Hover(_cancellationTokenSource.Token);
            }, _cancellationTokenSource.Token);
            task.ContinueWith(Cleanup);

            return Ok();
        }

        [HttpGet]
        [Route("/sendHome")]
        public IActionResult SendHome()
        {
            CheckTask();
            
            var task = Task.Run(async () => {
                await this._droneSim.SendHome(_cancellationTokenSource.Token);
            }, _cancellationTokenSource.Token);
            task.ContinueWith(Cleanup);

            return Ok();
        }

        [HttpPost]
        [Route("/sendOnMission")]
        public IActionResult SendOnMission([FromBody] List<Location> locations)
        {
            CheckTask();

            var task = Task.Run(async () => {
                if (_cancellationToken.IsCancellationRequested)
                    _cancellationToken.ThrowIfCancellationRequested();

                await this._droneSim.SendOnMission(locations, _cancellationTokenSource.Token);
            }, _cancellationTokenSource.Token);
            task.ContinueWith(Cleanup);

            return Ok();
        }

        [HttpPost]
        [Route("/landAtLocation")]
        public IActionResult LandAtLocation([FromBody] Location location)
        {
            CheckTask();

            var task = Task.Run(async () => {
                await this._droneSim.LandAtLocation(location, _cancellationTokenSource.Token);
            }, _cancellationTokenSource.Token);
            task.ContinueWith(Cleanup);

            return Ok();
        }

        private void CheckTask()
        {
            lock(_lock)
            {
                if (_cancellationTokenSource != null)
                {
                    _cancellationTokenSource.Cancel();
                    _cancellationTokenSource.Token.WaitHandle.WaitOne();
                }

                _cancellationTokenSource = new CancellationTokenSource();
            }
        }

        private static void Cleanup(Task task)
        {
            if (task != null)
            {
                System.Console.WriteLine(task.Exception.GetBaseException().Message);
            }

            lock (_lock)
            {
                if (_cancellationTokenSource != null)
                {
                    _cancellationTokenSource.Dispose();
                }
                _cancellationTokenSource = null;
            }
        }
    }
}
