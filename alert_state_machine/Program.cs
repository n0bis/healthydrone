using System;
using System.IO;
using System.Threading.Tasks;
using alert_state_machine.Services;
using Microsoft.Extensions.Configuration;
using alert_state_machine.RuleRunners;
using utm_service;
using Microsoft.Extensions.DependencyInjection;
using alert_state_machine.Persistence;
using alert_state_machine.Settings;
using AutoMapper;
using System.Threading;

namespace alert_state_machine
{
    class Program
    {
        // AutoResetEvent to signal when to exit the application.
        private static readonly AutoResetEvent waitHandle = new AutoResetEvent(false);

        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();

            var utmService = new UTMService(config["UTM:clientid"], config["UTM:clientsecret"],
                config["UTM:username"], config["UTM:password"]);

            var token = await utmService.Tokens.Auth();

            Scheduler.IntervalInMinutes(1, async () =>
            {
                Console.WriteLine($"Running weather check at: {DateTime.Now}");
                await serviceProvider.GetService<IWeatherRunner>().WeatherCheck(utmService);
            });

            Scheduler.IntervalInMinutes(0.5, async () =>
            {
                Console.WriteLine($"Running collision and no-flyzone check at: {DateTime.Now}");
                await serviceProvider.GetService<ICollisionAndNoFlyZoneRunner>().ZonesCheck(token, utmService);
            });

            // Handle Control+C or Control+Break
            Console.CancelKeyPress += (o, e) =>
            {
                Console.WriteLine("Exit");

                // Allow the manin thread to continue and exit...
                waitHandle.Set();
            };

            // Wait
            waitHandle.WaitOne();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // build config
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();

            services.AddOptions();
            services.Configure<RedisOpts>(configuration.GetSection("Redis"));
            services.Configure<WeatherRuleOpts>(configuration.GetSection("WeatherRule"));

            // add services:
            services.AddSingleton<IRedisService, RedisService>();
            services.AddScoped<IWeatherService, WeatherService>();
            services.AddScoped<IWeatherRunner, WeatherRunner>();
            services.AddScoped<ICollisionAndNoFlyZoneRunner, CollisionAndNoFlyZoneRunner>();
            services.AddSingleton<IUTMLiveService, UTMLiveService>();

            var mapper = new MapperConfiguration(mc => { }).CreateMapper();
            services.AddSingleton(mapper);
        }
    }

    
}