using System;
using System.Threading.Tasks;
using utm_service;

namespace alert_state_machine.RuleRunners
{
    public interface IWeatherRunner
    {
        Task WeatherCheck(UTMService utmService);
    }
}
