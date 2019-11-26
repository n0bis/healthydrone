using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using reportIncident.API.Domain.Repositories;
using reportIncident.API.Domain.Services;
using reportIncident.API.Persistence.Contexts;
using reportIncident.API.Persistence.Reposetories;
using reportIncident.API.Services;
using AutoMapper;
using reportIncident.API.Mapping;

namespace reportIncident.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddEntityFrameworkNpgsql().AddDbContext<AppDbContext>(options => {
                options.UseNpgsql(Configuration.GetConnectionString("AppDbContext"));
            });

            services.AddScoped<IIncidentsRepository, IncidentsRepository>();
            services.AddScoped<IIncidentsService, IncidentsService>();

            services.AddAutoMapper(typeof(Startup));
            
            

            services.AddScoped<IUnitOfWork, UnitOfWork>();

        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
