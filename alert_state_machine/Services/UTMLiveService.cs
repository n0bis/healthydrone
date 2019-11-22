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
        private IMapper _mapper;

        public UTMLiveService(IMapper mapper)
        {
            this.opts = new PureSocketClusterOptions
            {
                Creds = null, 
                MyReconnectStrategy = new ReconnectStrategy(500, 2000),
                DebugMode = true
            };
            this.utm = new PureSocketClusterSocket("wss://healthdrone.unifly.tech/socketcluster/", opts);

            _mapper = mapper;
        }

        public async Task Connect(string token)
        {
            this.utm.SetAuthToken(token);
            var channel = await this.utm.CreateChannel("adsb").SubscribeAsync();
            channel.OnMessage(OnMessage);
            await this.utm.ConnectAsync();
        }

        private void OnMessage(object sender, string name, object data)
        {
            //Console.WriteLine($"{((PureSocketClusterSocket)sender).InstanceName} {name} : {data} \r\n", ConsoleColor.Green);
            //Console.WriteLine(data);

            var test = (Dictionary<string, object>)(data);
            var testa = (IEnumerable<object>)test.Values.FirstOrDefault();
            if (test.Values.ElementAt(1).ToString() == "ADSB_TRACK")
                return;

            var result = JsonConvert.SerializeObject(DictionaryToObject(testa), Formatting.Indented);
            var obj = JsonConvert.DeserializeObject<Models.Message>(result);        

            Console.WriteLine(obj);
        }

        private static dynamic DictionaryToObject(IEnumerable<object> dictionary)
        {
            var expandoObj = new ExpandoObject();
            var expandoObjCollection = (ICollection<KeyValuePair<String, Object>>)expandoObj;

            foreach (var keyValuePair in dictionary)
            {
                var test = (Dictionary<string, object>)keyValuePair;
                foreach (var v in test)
                {
                    expandoObjCollection.Add(v);
                }
            }
            dynamic eoDynamic = expandoObj;
            return eoDynamic;
        }
    }

}