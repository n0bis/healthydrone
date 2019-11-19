using System;
using System.Threading.Tasks;
using alert_state_machine.Services;
using utm_service;
using utm_service.Models;

namespace alert_state_machine.RuleRunners
{
    public class CollisionAndNoFlyZoneRunner : ICollisionAndNoFlyZoneRunner
    {
        public CollisionAndNoFlyZoneRunner()
        {
        }

        public async Task ZonesCheck(Token token)
        {
            var utmLiveService = new UTMLiveService();
            await utmLiveService.Connect(token?.access_token);
        }
    }
}
