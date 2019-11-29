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

namespace alert_state_machine
{
    class Program
    {
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

            /*Scheduler.IntervalInMinutes(1, async () =>
            {
                Console.WriteLine(DateTime.Now);
                await serviceProvider.GetService<IWeatherRunner>().WeatherCheck(utmService);
                Console.WriteLine(DateTime.Now);
            });*/

            Scheduler.IntervalInMinutes(0.5, async () =>
            {
                Console.WriteLine(DateTime.Now);
                await serviceProvider.GetService<ICollisionAndNoFlyZoneRunner>().ZonesCheck(token, utmService);
                Console.WriteLine(DateTime.Now);
            });

            Console.ReadKey(true);
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