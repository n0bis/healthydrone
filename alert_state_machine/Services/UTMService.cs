using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using alert_state_machine.Models;
using Microsoft.Extensions.Configuration;

namespace alert_state_machine.Services
{
    public class UTMService
    {
        private readonly HttpClient client = new HttpClient();
        private readonly IConfiguration config;

        public UTMService(IConfiguration config)
        {
            this.config = config;
            this.client.BaseAddress = new Uri("https://healthdrone.unifly.tech");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task Auth()
        {
            var clientId = System.Text.Encoding.Unicode.GetBytes($"{config["clientid"]}:{config["clientsecret"]}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(clientId));
            var content = new FormUrlEncodedContent(
                new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("username", config["username"]),
                    new KeyValuePair<string, string>("password", config["password"]),
                    new KeyValuePair<string, string>("grant_type", "password")
                });
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded")
            {
                CharSet = "UTF-8"
            };
            var response = await client.PostAsJsonAsync(
                "/oauth/token", content);
            if (response.IsSuccessStatusCode) {
                var token = await response.Content.ReadAsAsync<Token>();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.accessToken);
                config["token"] = token.accessToken;
            }
        }

       public async Task<List<Flight>> GetFlightsAsync()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", config["token"]);
            var response = await client.GetAsync("/api/uasoperations/flights");
            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadAsAsync<List<Flight>>();
            }
            return null;
        }
    }
}
