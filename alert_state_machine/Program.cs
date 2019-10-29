using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using alert_state_machine.Services;
using Microsoft.Extensions.Configuration;
using utm_service;
using utm_service.Models;
using alert_state_machine.Models;
using alert_state_machine.Persistence;
using alert_state_machine.States;
using alert_state_machine.RuleRunners;

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

            Scheduler.IntervalInMinutes(3, () =>
            {
                WeatherRunner.WeatherCheck(config);
            });

            Console.ReadKey(true);
        }
    }

    
}