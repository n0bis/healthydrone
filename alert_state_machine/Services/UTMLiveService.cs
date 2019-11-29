using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using alert_state_machine.Models;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PureSocketCluster;
using PureWebSockets;


namespace alert_state_machine.Services
{
    public class UTMLiveService : IUTMLiveService
    {
        private readonly PureSocketClusterOptions opts;
        private readonly PureSocketClusterSocket utm;
        

        public UTMLiveService()
        {
            this.opts = new PureSocketClusterOptions
            {
                Creds = null, 
                MyReconnectStrategy = new ReconnectStrategy(500, 2000),
                DebugMode = true
            };
            this.utm = new PureSocketClusterSocket("wss://healthdrone.unifly.tech/socketcluster/", opts);
        }

        public async Task Connect(string token, Action<object, string, object> action)
        {
            this.utm.SetAuthToken(token);
            var channel = await this.utm.CreateChannel("adsb").SubscribeAsync();
            channel.OnMessage((sender, name, data) => action(sender, name, data));
            await this.utm.ConnectAsync();
        }


    }

}