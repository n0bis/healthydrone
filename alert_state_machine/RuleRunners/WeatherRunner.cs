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
        private readonly WeatherRule _weatherRule;

        public WeatherRunner(IRedisService redisService, IWeatherService weatherService, IOptions<WeatherRuleOpts> weatherOpts)
        {
            _redisService = redisService;
            _weatherService = weatherService;
            _redisService.Connect();
            _weatherRule = new WeatherRule { MaxTemp = weatherOpts.Value.MaxTemp, MinTemp = weatherOpts.Value.MinTemp, RainPrecipitation = weatherOpts.Value.RainPrecipitation, WindSpeed = weatherOpts.Value.WindSpeed };
        }

        public async Task WeatherCheck(UTMService utmService)
        {
            List<Flight> flights = await utmService.Operation.GetFlightsInAllOperationsAsync();
            var distinctFlights = flights?.GroupBy(flight => flight.uas.uniqueIdentifier).Select(uas => uas.First()).ToList();
            distinctFlights?.ForEach(async flight =>
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

                var validatedRule = _weatherRule.ValidateRule(weatherResponse);
                if (!validatedRule.Success && (process.CurrentState == ProcessState.Active || process.CurrentState == ProcessState.Inactive))
                {
                    process.MoveNext();
                } else {
                    process.MovePrev();
                }
                
                if (process.CurrentState == ProcessState.Raised && !cachedProcess.Triggered && !cachedProcess.Handled) {
                    cachedProcess.Triggered = true;
                    await SendAlert(new Alert { droneId = flight.uas.uniqueIdentifier, type = "weather-alert", reason = validatedRule.Message });
                }
                cachedProcess.CurrentState = process.CurrentState;
                await _redisService.Set(key, cachedProcess);
            });
        }


		// localhost:9092
		private async Task SendAlert(object value)
		{
            Console.WriteLine($"ALERT: {JsonConvert.SerializeObject(value)}");
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                await producer.ProduceAsync("weather-alert", new Message<Null, string> { Value = JsonConvert.SerializeObject(value) });
            }
        }
    }
}
