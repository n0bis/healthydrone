﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using utm_service.Clients;
using utm_service.Models;

namespace utm_service
{
    public class UTMService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private bool _disposeHttpClient;

        public TokenClient Tokens { get; }
        public OperationClient Operation { get; }
        public TrackingClient Tracking { get; }

        public UTMService(string clientId, string clientSecret,
            string operatorMail, string operatorPass)
        {
            this._disposeHttpClient = true;
            this._httpClient = new HttpClient();
            this._httpClient.BaseAddress = new Uri("https://healthdrone.unifly.tech");
            this._httpClient.DefaultRequestHeaders.Clear();
            this._httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            this.Tokens = new TokenClient(_httpClient, new Credentials { ClientId = clientId, ClientSecret = clientSecret, OperatorMail = operatorMail, OperatorPass = operatorPass });
            Token token = null;
            Task.Run(async () => token = await Tokens.Auth()).Wait();
            if (token == null)
                throw new Exception("Authentication went wrong");

            this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.access_token);

            this.Operation = new OperationClient(_httpClient);
            this.Tracking = new TrackingClient(_httpClient);
        }

        public void Dispose()
        {
            if (this._disposeHttpClient)
            {
                this._httpClient.Dispose();
                this._disposeHttpClient = false;
            }
        }
    }
}
