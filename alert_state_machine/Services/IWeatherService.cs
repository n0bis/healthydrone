using System;
using System.Threading.Tasks;
using alert_state_machine.Models;

namespace alert_state_machine.Services
{
    public interface IWeatherService
    {
        Task<WeatherResponse> GetWeatherAtCoord(string latitude, string longitude);
        void Dispose();
    }
}
