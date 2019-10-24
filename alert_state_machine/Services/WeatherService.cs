using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using alert_state_machine.Models;

namespace alert_state_machine.Services
{
    public class WeatherService
    {
        private readonly HttpClient client = new HttpClient();

        public WeatherService()
        {
            this.client.BaseAddress = new Uri("https://api.openweathermap.org");
            this.client.DefaultRequestHeaders.Accept.Clear();
            this.client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<WeatherResponse> GetWeatherAtCoord(string latitude, string longitude)
        {
            var response = await this.client.GetAsync($"/data/2.5/weather?APPID=83845ade71566a7beda7c293096d8ed2&units=metric&lat={latitude}&lon={longitude}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<WeatherResponse>();
            }
            return null;
        }
    }
}
