using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using reportIncident.API.Domain.Repositories;
using reportIncident.API.Domain.Services;
using reportIncident.API.Persistence.Contexts;
using reportIncident.API.Persistence.Reposetories;
using reportIncident.API.Services;
using Microsoft.EntityFrameworkCore;

    

namespace reportIncident.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

       
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer("reportincident-api-in-memory")
            })
            });

            services.AddScoped<IIncidentsRepository, IncidentsRepository>();
            services.AddScoped<IIncidentsService, IncidentsService>();
        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
           

            

            
            });
        }
    }
}
