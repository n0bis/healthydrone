using System;
using System.Threading.Tasks;
using utm_service.Models;

namespace alert_state_machine.RuleRunners
{
    public interface ICollisionAndNoFlyZoneRunner
    {
        Task ZonesCheck(Token token);
    }
}
