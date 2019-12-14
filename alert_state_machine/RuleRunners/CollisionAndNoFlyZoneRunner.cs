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
using System.Collections;
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
        private readonly string _kafkaHost;

        public CollisionAndNoFlyZoneRunner(IRedisService redisService, IUTMLiveService utmLiveService, IOptions<KafkaOpts> kafkaOpts)
        {
            _redisService = redisService;
            _redisService.Connect();
            _UTMLiveService = utmLiveService;
            _kafkaHost = kafkaOpts.Value.Host;
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
            var dict = (Dictionary<string, object>)(data);
            var json = JsonConvert.SerializeObject(dict, Formatting.Indented);
            var message = JsonConvert.DeserializeObject<EventMessage>(json, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            });

            if (message == null || message.name == "ADSB_TRACK")
                return;

            var dataObj = message.data.FirstOrDefault();

            if (dataObj == null || dataObj.alertType == "OUTSIDE_OPERATION" || dataObj.alertType == "NO_OPERATION")
                return;

            var key = $"{dataObj.subject.uniqueIdentifier}-{dataObj.relatedSubject.uniqueIdentifier}-alert";
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

                if (dataObj.alertType == "UAS_NOFLYZONE")
                {
                    await SendAlert(new Alert { droneId = dataObj.subject.uniqueIdentifier, type = "no-fly-zone-alert", reason = "Out of Bounds" });
                    cachedProcess.Triggered = true;
                }

                if (dataObj.alertType == "UAS_COLLISION")
                {
                    await SendAlert(new Alert { droneId = dataObj.subject.uniqueIdentifier, type = "collision-alert", reason = "Collision" });
                    cachedProcess.Triggered = true;
                }
            }
            cachedProcess.CurrentState = process.CurrentState;
            await _redisService.Set(key, cachedProcess);
        }

        private async Task SendAlert(object value)
        {
            Console.WriteLine($"ALERT: {JsonConvert.SerializeObject(value)}");
            var config = new ProducerConfig { BootstrapServers = $"{_kafkaHost}:9092" };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                await producer.ProduceAsync("collision-alert", new Message<Null, string> { Value = JsonConvert.SerializeObject(value) });
            }
        }
    }
}
