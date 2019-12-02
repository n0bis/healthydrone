using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using alert_state_machine.Models;
using alert_state_machine.States;
using HandleAlerts.API.Domain.Models;
using HandleAlerts.API.Persistence;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HandleAlerts.API.Controllers
{
    [Route("api/[controller]")]
    public class AlertsController : Controller
    {
        IRedisService _redisService;

        public AlertsController(IRedisService redisService)
        {
            _redisService = redisService;
            _redisService.Connect();
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(Guid uasOperation, Guid droneID, string alertType )
        {
            var process = new Process();

            var key = $"{uasOperation}-{droneID}-{alertType}";

            var cachedProcess = await _redisService.Get<State>(key);


            process.CurrentState = cachedProcess.CurrentState;
            cachedProcess.Triggered = false;
            cachedProcess.Handled = true;


            await _redisService.Set(key, cachedProcess);

            return Ok();
        }

        // TODO
        // implement post endpoint for request drone
    }


}
