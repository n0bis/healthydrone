using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using alert_state_machine.Models;
using alert_state_machine.Persistence;
using alert_state_machine.Services;
using AutoMapper;
using Confluent.Kafka;
using Newtonsoft.Json;
using utm_service;
using utm_service.Models;

namespace alert_state_machine.RuleRunners
{
    public class CollisionAndNoFlyZoneRunner : ICollisionAndNoFlyZoneRunner
    {
        private readonly IRedisService _redisService;
        private Boolean isConnected = false;
        private readonly IUTMLiveService _UTMLiveService;
        private Models.Message _ObjectData;
        private UTMService _utmService; 

        public CollisionAndNoFlyZoneRunner(IRedisService redisService, IUTMLiveService utmLiveService)
        {
            _redisService = redisService;
            _redisService.Connect();
            _UTMLiveService = utmLiveService;
        }

        public async Task ZonesCheck(Token token)
        {
            if (isConnected == false)
            {
                var action = new Action<object, string, object>(OnMessage);

                await _UTMLiveService.Connect(token?.access_token, action);
                isConnected = true;

            }
        }

        private async void OnMessage(object sender, string name, object data)
        {
            //Console.WriteLine($"{((PureSocketClusterSocket)sender).InstanceName} {name} : {data} \r\n", ConsoleColor.Green);
            //Console.WriteLine(data);

            List<Flight> flights = await _utmService.Operation.GetFlightsInAllOperationsAsync();
            var distinctFlights = flights?.GroupBy(flight => flight.uas.uniqueIdentifier).Select(uas => uas.First()).ToList();

            var test = (Dictionary<string, object>)(data);
            if (test.Values.ElementAt(1).ToString() == "ADSB_TRACK")
                return;

            try
            {

                var testa = (IEnumerable<object>)test.Values.FirstOrDefault();

                var result = JsonConvert.SerializeObject(DictionaryToObject(testa), Formatting.Indented);
                var obj = JsonConvert.DeserializeObject<Models.Message>(result);

                Console.WriteLine(obj);

                _ObjectData = obj;

                if (_ObjectData.alertType.Equals("UAS_COLLISION") || _ObjectData.alertType.Equals("UAS_NOFLYZONE"))
                {
                    distinctFlights?.ForEach(async flight =>
                    {
                        if (_ObjectData.subject.uniqueIdentifier == flight.uas.uniqueIdentifier)
                        {
                            await SendAlert(new Alert { droneId = flight.uas.uniqueIdentifier, type = "collision-alert", reason = "Out of Bounds" });
                        }

                    });
                }


            }
            catch (Exception e)
            {

            }

        }

        private async Task SendAlert(object value)
        {
            Console.WriteLine($"ALERT: {JsonConvert.SerializeObject(value)}");
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                await producer.ProduceAsync("collision-alert", new Message<Null, string> { Value = JsonConvert.SerializeObject(value) });
            }
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
