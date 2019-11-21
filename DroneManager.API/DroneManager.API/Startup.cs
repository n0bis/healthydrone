using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DroneManager.API.Configuration;
using DroneManager.API.Domain.Repositories;
using DroneManager.API.Domain.Services;
using DroneManager.API.Mapping;
using DroneManager.API.Persistence.Contexts;
using DroneManager.API.Persistence.Repositories;
using DroneManager.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DroneManager.API
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

            services.AddEntityFrameworkNpgsql().AddDbContext<AppDbContext>(
                opt => opt.UseNpgsql(Configuration.GetConnectionString("AppDbContext")));

            services.AddScoped<IDroneRepository, DroneRepository>();
            services.AddScoped<IDroneService, DroneService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.Configure<UTMOpts>(Configuration.GetSection("UTM"));

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ModelToResourceProfile());
                mc.AddProfile(new ResourceToModelProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
