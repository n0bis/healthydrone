using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PureSocketCluster;
using PureWebSockets;


namespace alert_state_machine.Services
{
    public class UTMLiveService
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

        public async Task Connect(string token)
        {
            this.utm.SetAuthToken(token);
            var channel = await this.utm.CreateChannel("adsb").SubscribeAsync();
            channel.OnMessage(OnMessage);
            await this.utm.ConnectAsync();
        }

        private static void OnMessage(object sender, string name, object data)
        {
            //Console.WriteLine($"{((PureSocketClusterSocket)sender).InstanceName} {name} : {data} \r\n", ConsoleColor.Green);
            var obj = JsonConvert.DeserializeObject<Message>(data.ToString());
            Console.WriteLine(obj);
        }
    }
}