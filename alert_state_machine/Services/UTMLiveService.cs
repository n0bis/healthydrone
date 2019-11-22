using System;
using System.Collections.Generic;
using System.Dynamic;
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

            String result = JsonConvert.SerializeObject(DictionaryToObject(test), Formatting.Indented);

//          DataOuter obj = _mapper.Map<object, DataOuter>(data);            

            Console.WriteLine(result);
        }
        private static dynamic DictionaryToObject(IDictionary<String, Object> dictionary)
        {
            var expandoObj = new ExpandoObject();
            var expandoObjCollection = (ICollection<KeyValuePair<String, Object>>)expandoObj;

            foreach (var keyValuePair in dictionary)
            {
                expandoObjCollection.Add(keyValuePair);
            }
            dynamic eoDynamic = expandoObj;
            return eoDynamic;
        }
    }

}