using System;
using System.IO;
using System.Threading.Tasks;
using alert_state_machine.Services;
using Microsoft.Extensions.Configuration;
using alert_state_machine.RuleRunners;
using utm_service;

namespace alert_state_machine
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var config = builder.Build();

            var utmService = new UTMService(config["UTM:clientid"], config["UTM:clientsecret"],
                config["UTM:username"], config["UTM:password"]);

            Scheduler.IntervalInMinutes(1, async () =>
            {
                Console.WriteLine(DateTime.Now);
                await WeatherRunner.WeatherCheck(config, utmService);
                Console.WriteLine(DateTime.Now);
            });

            Console.ReadKey(true);
        }
    }

    
}