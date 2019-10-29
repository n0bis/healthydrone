using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using alert_state_machine.Models;
using alert_state_machine.Services;
using Microsoft.Extensions.Configuration;

namespace alert_state_machine
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var config = builder.Build();

            var utmService = new UTMService(config);
            await utmService.Auth();

            List<Flight> flights = await utmService.GetFlightsAsync();
            flights.ForEach(flight => Console.WriteLine(flight));

            //var weatherSerivce = new WeatherService();

            //var weather = await weatherSerivce.GetWeatherAtCoord(latitude: "55.3604864174024", longitude: "10.438549248423866");
            //Console.WriteLine(weather);
            Scheduler.IntervalInMinutes(1, () =>
            {
                Console.WriteLine(DateTime.Now);
            });

            Scheduler.IntervalInHours(12, () =>
            {
                Console.WriteLine("Hello");
            });

            Console.ReadKey(true);
        }
    }

    
}