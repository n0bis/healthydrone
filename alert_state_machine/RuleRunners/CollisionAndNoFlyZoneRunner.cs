using System;
using System.Threading.Tasks;
using alert_state_machine.Persistence;
using alert_state_machine.Services;
using utm_service;
using utm_service.Models;

namespace alert_state_machine.RuleRunners
{
    public class CollisionAndNoFlyZoneRunner : ICollisionAndNoFlyZoneRunner
    {
        private readonly IRedisService _redisService;
        private Boolean isConnected = false;
        private readonly IUTMLiveService _UTMLiveService;

        public CollisionAndNoFlyZoneRunner(IRedisService redisService, IUTMLiveService utmLiveService)
        {
            _redisService = redisService;
            _redisService.Connect();
            _UTMLiveService = utmLiveService;

        }

        public async Task ZonesCheck(Token token)
        {
            if (isConnected == false)
            {
                await _UTMLiveService.Connect(token?.access_token);
                isConnected = true;
            }

        }
    }
}
