using System;
using System.Net.Http;
using System.Net.Http.Headers;
using common.utm.service.Clients;
using common.utm.service.Models;

namespace common.utm.service
{
    public class UTMService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private bool _disposeHttpClient;

        public TokenClient Tokens { get; }
        public OperationClient Operation { get; }

        public UTMService(string clientId, string clientSecret,
            string operatorMail, string operatorPass)
        {
            this._disposeHttpClient = true;
            this._httpClient = new HttpClient();
            this._httpClient.BaseAddress = new Uri("");
            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            this.Tokens = new TokenClient(_httpClient, new Credentials { ClientId = clientId, ClientSecret = clientSecret, OperatorMail = operatorMail, OperatorPass = operatorPass });
            this.Operation = new OperationClient(_httpClient);

            this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.Tokens.Auth().Result.access_token);
        }

        public void Dispose()
        {
            if(this._disposeHttpClient)
            {
                this._httpClient.Dispose();
                this._disposeHttpClient = false;
            }
        }
    }
}
