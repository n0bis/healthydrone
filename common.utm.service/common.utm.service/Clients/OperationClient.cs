﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using common.utm.service.Models;

namespace common.utm.service.Clients
{
    public class OperationClient : ClientBase
    {
        public OperationClient(HttpClient httpClient)
            : base(httpClient)
        {
        }

        public async Task<List<Flight>> GetFlightsInAllOperationsAsync()
        {
            var response = await HttpClient.GetAsync("/api/uasoperations/flights");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<Flight>>();
            }
            return new List<Flight>();
        }
    }
}
