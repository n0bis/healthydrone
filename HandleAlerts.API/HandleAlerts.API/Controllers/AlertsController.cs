using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using alert_state_machine.Models;
using alert_state_machine.States;
using HandleAlerts.API.Domain.Models;
using HandleAlerts.API.Hubs;
using HandleAlerts.API.Persistence;
using HandleAlerts.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HandleAlerts.API.Controllers
{
    [Route("api/[controller]")]
    public class AlertsController : Controller
    {
        private readonly IRedisService _redisService;
        private readonly IHubContext<AlertHub> _hubContext;

        public AlertsController(IRedisService redisService, IHubContext<AlertHub> hubContext)
        {
            _redisService = redisService;
            _redisService.Connect();
            _hubContext = hubContext;
        }

        [HttpPost]
        [Route("/HandleAlert")]
        public async Task<IActionResult> PostAsync(HandleAlertResource resource)
        {
            var key = $"{resource.UasOperation}-{resource.DroneID}-{resource.AlertType}";

            var cachedProcess = await _redisService.Get<State>(key);
            cachedProcess.Handled = true;

            await _redisService.Set(key, cachedProcess);

            return Ok();
        }

        // TODO
        // implement post endpoint for request drone
        [HttpPost]
        [Route("/RequetDrone")]
        public async Task<IActionResult> RequestDrone(RequestDroneResource resource)
        {
            await _hubContext.Clients.All.SendAsync("request_drone", resource);

            return Accepted();
        }
    }
}
