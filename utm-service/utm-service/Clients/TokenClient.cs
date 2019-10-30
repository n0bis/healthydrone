using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using utm_service.Models;

namespace utm_service.Clients
{
    public class TokenClient : ClientBase
    {
        private Credentials _credentials;

        public TokenClient(HttpClient httpClient, Credentials credentials)
            : base(httpClient)
        {
            _credentials = credentials;
        }

        public async Task<Token> Auth()
        {
            var clientIdBytes = Encoding.GetEncoding(28591).GetBytes($"{_credentials.ClientId}:{_credentials.ClientSecret}");
            string encodedClientId = Convert.ToBase64String(clientIdBytes);

            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedClientId);

            var request = new HttpRequestMessage(HttpMethod.Post, "/oauth/token");
            request.Content = new StringContent(
                $"username={_credentials.OperatorMail}&password={_credentials.OperatorPass}&grant_type=password",
                Encoding.UTF8,
                "application/x-www-form-urlencoded");

            var response = await HttpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<Token>();
            }
            return null;
        }
    }
}
