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

        public CollisionAndNoFlyZoneRunner(IRedisService redisService)
        {
            _redisService = redisService;
            _redisService.Connect();

        }

        public async Task ZonesCheck(Token token)
        {
            if (isConnected == false)
            {
                var utmLiveService = new UTMLiveService();
                await utmLiveService.Connect(token?.access_token);
                isConnected = true;
            }

        }
    }
}
