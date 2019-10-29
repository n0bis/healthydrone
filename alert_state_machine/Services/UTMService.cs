using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
            this.client.DefaultRequestHeaders.Accept.Clear();
            this.client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /*
        curl --location --request POST "{{ENV_BASEURL}}/oauth/token" \
          --header "Content-Type: application/x-www-form-urlencoded" \
          --header "Accept: application/json" \
          --header "Authorization: Basic {{CREDENTIALS_ENCODED}}" \
          --data "username={{OPERATOR_MAIL}}&password={{OPERATOR_PASS}}&grant_type=password"
        */
        public async Task Auth()
        {
            var clientId = Encoding.GetEncoding(28591).GetBytes($"{config["UTM:clientid"]}:{config["UTM:clientsecret"]}");
            string clientIdBase64 = Convert.ToBase64String(clientId);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", clientIdBase64);

            var request = new HttpRequestMessage(HttpMethod.Post, "/oauth/token");
            request.Content = new StringContent($"username={config["UTM:username"]}&password={config["UTM:password"]}&grant_type=password",
                Encoding.UTF8,
                "application/x-www-form-urlencoded");
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode) {
                var token = await response.Content.ReadAsAsync<Token>();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.access_token);
                config["UTM:token"] = token.access_token;
            }
        }

       public async Task<List<Flight>> GetFlightsAsync()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", config["UTM:token"]);
            var response = await client.GetAsync("/api/uasoperations/flights");
            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadAsAsync<List<Flight>>();
            }
            return null;
        }
    }
}
