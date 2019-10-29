using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using alert_state_machine.Models;
using alert_state_machine.Persistence;
using alert_state_machine.Services;
using alert_state_machine.States;
using Microsoft.Extensions.Configuration;
using utm_service;
using utm_service.Models;

namespace alert_state_machine.RuleRunners
{
    public static class WeatherRunner
    {
        public static async Task WeatherCheck(IConfiguration config)
        {
            using (var utmService = new UTMService(config["UTM:clientid"], config["UTM:clientsecret"],
                config["UTM:username"], config["UTM:password"]))
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
                    if (cachedProcess != null) {
                        process.CurrentState = cachedProcess.CurrentState;
                    } else {
                        cachedProcess = new State();
                    }

                    if (weatherResponse.main.temp < 20 && (process.CurrentState == ProcessState.Active || process.CurrentState == ProcessState.Inactive)) {
                        process.MoveNext();
                    } else {
                        process.MovePrev();
                    }

                    
                    if (process.CurrentState == ProcessState.Raised && !cachedProcess.Triggered && !cachedProcess.Handled) {
                        Console.WriteLine($"SEND ALERT FOR: {flight.uasOperation}-{flight.uas.uniqueIdentifier}-weather");
                        cachedProcess.Triggered = true;
                    }
                    cachedProcess.CurrentState = process.CurrentState;
                    await redisService.Set(key, cachedProcess);
                });
            }
        }
    }
}
