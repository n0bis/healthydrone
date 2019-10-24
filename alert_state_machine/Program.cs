using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using alert_state_machine.Models;
using alert_state_machine.Services;
using Microsoft.Extensions.Configuration;

namespace alert_state_machine
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var config = builder.Build();


            /*var opts = new PureWebSocketOptions { MyReconnectStrategy = new ReconnectStrategy(500, 2000), DebugMode = true };
            var weather = new PureWebSocket("wss://swd.weatherflow.com/swd/data?api_key=a8f5dbda-af0a-4b57-99b9-f10baa88f27b", opts);
            weather.OnMessage += (sender, message) => Console.WriteLine($"{sender} : {message}", ConsoleColor.Green);
            await weather.ConnectAsync();
            await weather.SendAsync("{\"type\":\"listen_start\", \"device_id\":5356}");
            await weather.SendAsync("{\"type\":\"listen_rapid_start\", \"device_id\":5356}");
            await weather.SendAsync("{\"type\":\"listen_start\", \"device_id\":12202}");
            await weather.SendAsync("{\"type\":\"listen_rapid_start\", \"device_id\":12202}");*/

            /*var utmService = new UTMService(config);
            await utmService.Auth();

            List<Flight> flights = await utmService.GetFlightsAsync();
            flights.ForEach(flight => Console.WriteLine(flight));*/

            var weatherSerivce = new WeatherService();

            var weather = await weatherSerivce.GetWeatherAtCoord(latitude: "55.3604864174024", longitude: "10.438549248423866");
            Console.WriteLine(weather);
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