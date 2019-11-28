using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Docker.DotNet;
using DroneManager.API.Domain.Services;
using DroneManager.API.Persistence.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DroneManager.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            /*var dockerClient = new DockerClientConfiguration(DockerApiUri())
                .CreateClient();

            using (var scope = host.Services.CreateScope())
            using(var context = scope.ServiceProvider.GetService<AppDbContext>())
            {
                context.Database.EnsureCreated();
                var containers = context.DockerContainers.ToList();
                containers.ForEach(async container => await dockerClient.Containers.StartContainerAsync(container.Id, null));
            }*/

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static Uri DockerApiUri()
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            var isMac = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

            if (isWindows)
            {
                return new Uri("npipe://./pipe/docker_engine");
            }

            if (isLinux || isMac)
            {
                return new Uri("unix:/var/run/docker.sock");
            }

            throw new Exception("Was unable to determine what OS this is running");
        }
    }
}
