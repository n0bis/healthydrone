using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using alert_state_machine.Models;
using alert_state_machine.Persistence;
using alert_state_machine.Services;
using alert_state_machine.States;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using utm_service;
using utm_service.Models;

namespace alert_state_machine.RuleRunners
{
    public static class WeatherRunner
    {
        public static async Task WeatherCheck(IConfiguration config, UTMService utmService)
        {
            var weatherService = new WeatherService();
            var redisService = new RedisService(config);
            List<Flight> flights = await utmService.Operation.GetFlightsInAllOperationsAsync();
            flights?.ForEach(async flight =>
            {
                var flightCoordinates = flight.coordinate;
                var weatherResponse = await weatherService.GetWeatherAtCoord(latitude: flightCoordinates.latitude.ToString(), longitude: flightCoordinates.longitude.ToString());
                redisService.Connect();
                var process = new Process();
                var key = $"{flight.uasOperation}-{flight.uas.uniqueIdentifier}-weather";
                var cachedProcess = await redisService.Get<State>(key);
                if (cachedProcess != null)
                {
                    process.CurrentState = cachedProcess.CurrentState;
                }
                else
                {
                    cachedProcess = new State();
                }

                //Console.WriteLine(weatherResponse.rain?.precipitation.ToString());

                if (weatherResponse.main.temp < -15 && (process.CurrentState == ProcessState.Active || process.CurrentState == ProcessState.Inactive)) {
                    process.MoveNext();
                } else {
                    process.MovePrev();
                }

                


                if (process.CurrentState == ProcessState.Raised && !cachedProcess.Triggered && !cachedProcess.Handled) {
                    SendAlert(cachedProcess);
                    cachedProcess.Triggered = true;
                }
                cachedProcess.CurrentState = process.CurrentState;
                await redisService.Set(key, cachedProcess);
            });
        }


		// localhost:9092
		private static void SendAlert(object value)
		{
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                producer.Produce("weather-alert", new Message<Null, string> { Value = JsonConvert.SerializeObject(value) });

                producer.Flush(TimeSpan.FromMilliseconds(100));
            }
        }
    }
}
