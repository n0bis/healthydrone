using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using alert_state_machine.Models;

namespace alert_state_machine.Services
{
    public class WeatherService : IDisposable, IWeatherService
    {
        private readonly HttpClient _client = new HttpClient();
        private bool _disposeHttpClient;

        public WeatherService()
        {
            this._disposeHttpClient = true;
            this._client.BaseAddress = new Uri("https://api.openweathermap.org");
            this._client.DefaultRequestHeaders.Accept.Clear();
            this._client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<WeatherResponse> GetWeatherAtCoord(string latitude, string longitude)
        {
            var response = await this._client.GetAsync($"/data/2.5/weather?APPID=83845ade71566a7beda7c293096d8ed2&units=metric&lat={latitude}&lon={longitude}");
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<WeatherResponse>();
                }
                return null;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public void Dispose()
        {
            if(this._disposeHttpClient)
            {
                this._client.Dispose();
                this._disposeHttpClient = false;
            }
        }
    }
}
