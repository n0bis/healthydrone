using System;
using System.Net.Http;

namespace utm_service.Clients
{
    public class ClientBase
    {
        internal HttpClient HttpClient { get; }

        internal ClientBase(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
        }
    }
}
