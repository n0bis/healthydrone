using alert_state_machine.Models;
using alert_state_machine.Persistence;
using alert_state_machine.Services;
using alert_state_machine.States;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using utm_service;
using utm_service.Models;

namespace alert_state_machine.RuleRunners
{
    public class WeatherRunner : IWeatherRunner
    {
        private readonly IRedisService _redisService;
        private readonly IWeatherService _weatherService;

        public WeatherRunner(IRedisService redisService, IWeatherService weatherService)
        {
            _redisService = redisService;
            _weatherService = weatherService;
            _redisService.Connect();
        }

        public async Task WeatherCheck(UTMService utmService)
        {
            List<Flight> flights = await utmService.Operation.GetFlightsInAllOperationsAsync();
            flights?.ForEach(async flight =>
            {
                var flightCoordinates = flight.coordinate;
                var weatherResponse = await _weatherService.GetWeatherAtCoord(latitude: flightCoordinates.latitude.ToString(), longitude: flightCoordinates.longitude.ToString());
                var process = new Process();
                var key = $"{flight.uasOperation}-{flight.uas.uniqueIdentifier}-weather";
                var cachedProcess = await _redisService.Get<State>(key);
                if (cachedProcess != null)
                {
                    process.CurrentState = cachedProcess.CurrentState;
                }
                else
                {
                    cachedProcess = new State();
                }

                if ((weatherResponse.main.temp < -15 || weatherResponse?.rain?.precipitation > 7 ) && (process.CurrentState == ProcessState.Active || process.CurrentState == ProcessState.Inactive))
                {
                    process.MoveNext();
                } else {
                    process.MovePrev();
                }
                
                if (process.CurrentState == ProcessState.Raised && !cachedProcess.Triggered && !cachedProcess.Handled) {
                    cachedProcess.Triggered = true;
                    SendAlert(new Alert { droneId = flight.uas.uniqueIdentifier, type = "weather-alert" });
                }
                cachedProcess.CurrentState = process.CurrentState;
                await _redisService.Set(key, cachedProcess);
            });
        }


		// localhost:9092
		private void SendAlert(object value)
		{
            Console.WriteLine("ALERT");
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                producer.Produce("test", new Message<Null, string> { Value = JsonConvert.SerializeObject(value) });

                producer.Flush(TimeSpan.FromMilliseconds(100));
            }
        }
    }
}
