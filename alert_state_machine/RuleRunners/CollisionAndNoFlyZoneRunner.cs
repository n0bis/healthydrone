using alert_state_machine.Models;
using alert_state_machine.Persistence;
using alert_state_machine.Services;
using alert_state_machine.Settings;
using alert_state_machine.States;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using utm_service;
using utm_service.Models;

namespace alert_state_machine.RuleRunners
{
    public class CollisionAndNoFlyZoneRunner : ICollisionAndNoFlyZoneRunner
    {
        private readonly IRedisService _redisService;
        private Boolean isConnected = false;
        private readonly IUTMLiveService _UTMLiveService;
        private UTMService _utmService; 

        public CollisionAndNoFlyZoneRunner(IRedisService redisService, IUTMLiveService utmLiveService)
        {
            _redisService = redisService;
            _redisService.Connect();
            _UTMLiveService = utmLiveService;
        }

        public async Task ZonesCheck(Token token, UTMService utmService)
        {
            if (isConnected == false)
            {
                _utmService = utmService;

                var action = new Action<object, string, object>(OnMessage);

                await _UTMLiveService.Connect(token?.access_token, action);
                isConnected = true;

            }
        }

        private async void OnMessage(object sender, string name, object data)
        {
            var test = (Dictionary<string, object>)(data);
            if (test.Values.ElementAt(1).ToString() == "ADSB_TRACK" || test == null)
                return;

            try
            {
                var testa = (IEnumerable<object>)test.Values.FirstOrDefault();

                var result = JsonConvert.SerializeObject(DictionaryToObject(testa), Formatting.Indented);
                var obj = JsonConvert.DeserializeObject<Models.Message>(result, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                });
                if (obj.alertType == "OUTSIDE_OPERATION" || obj == null)
                    return;

                var key = $"{obj.subject.uniqueIdentifier}-{obj.relatedSubject.uniqueIdentifier}-alert";
                var cachedProcess = await _redisService.Get<State>(key);
                var process = new Process();

                if (cachedProcess != null)
                {
                    process.CurrentState = cachedProcess.CurrentState;
                }
                else
                {
                    cachedProcess = new State();
                }

                if (!cachedProcess.Triggered && !cachedProcess.Handled)
                {

                    if (obj.alertType == "UAS_NOFLYZONE")
                    {
                        await SendAlert(new Alert { droneId = obj.subject.uniqueIdentifier, type = "no-fly-zone-alert", reason = "Out of Bounds" });
                        cachedProcess.Triggered = true;
                    }

                    if (obj.alertType == "UAS_NOFLYZONE")
                    {
                        await SendAlert(new Alert { droneId = obj.subject.uniqueIdentifier, type = "collision-alert", reason = "Collision" });
                        cachedProcess.Triggered = true;
                    }
                }
                cachedProcess.CurrentState = process.CurrentState;
                await _redisService.Set(key, cachedProcess);

            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(test);
                Console.WriteLine(e.Message);
            }
        }

        private async Task SendAlert(object value)
        {
            Console.WriteLine($"ALERT: {JsonConvert.SerializeObject(value)}");
            var config = new ProducerConfig { BootstrapServers = "kafka:9092" };

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
