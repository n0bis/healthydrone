using System.Net.Http;
using common.utm.service.Models;

namespace common.utm.service.Clients
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
