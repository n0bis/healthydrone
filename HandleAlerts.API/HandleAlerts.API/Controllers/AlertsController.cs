using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public Alert Post(Guid uasOperation, Guid droneID, string alertType )
        {
            var key = $"{uasOperation}-{droneID}-{alertType}";
            _redisService.Set(key, alertType);
        }
    }


}
