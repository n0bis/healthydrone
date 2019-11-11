using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using utm_service.Models;

namespace utm_service.Clients
{
    public class TrackingClient : ClientBase
    {
        public TrackingClient(HttpClient httpClient)
            : base(httpClient)
        {
        }

        public async Task<bool> FlightTrack(string droneId, string operationId, Track track)
        {
            var json = JsonConvert.SerializeObject(track);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync($"/api/uasoperations/{operationId}/uases/{droneId}/track", content);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> TakeOff(string droneId, string operationId)
        {
            var content = new StringContent($"{{endTime: {DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")} }}", Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync($"/api/uasoperations/${operationId}/uases/${droneId}/takeoff", content);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Land(string droneId, string operationId)
        {
            var content = new StringContent($"{{endTime: {DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")} }}", Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync($"/api/uasoperations/${operationId}/uases/${droneId}/landing", content);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
