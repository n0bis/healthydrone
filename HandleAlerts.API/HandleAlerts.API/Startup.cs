using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HandleAlerts.API.Domain.Services;
using HandleAlerts.API.Hubs;
using HandleAlerts.API.Services;
using HandleAlerts.API.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HandleAlerts.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.Configure<KafkaOpts>(Configuration.GetSection("Kafka"));
            services.AddSingleton<IAlertConsumer, AlertConsumer>();
            services.AddSingleton<IAlertStream, AlertStream>();
            services.AddSingleton<AlertRelay>();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyMethod();
                });
            });

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<AlertHub>("/alerts");
            });

            app.ApplicationServices.GetService<AlertRelay>();
            Task.Run(() => app.ApplicationServices.GetService<IAlertConsumer>().Listen());
        }
    }
}
