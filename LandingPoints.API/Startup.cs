using AutoMapper;
using LandingPoints.API.Domain.Repositories;
using LandingPoints.API.Domain.Services;
using LandingPoints.API.Persistance.Contexts;
using LandingPoints.API.Persistance.Repositories;
using LandingPoints.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LandingPoints.API.Mapping;

namespace LandingPoints.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddControllers();

            services.AddEntityFrameworkNpgsql().AddDbContext<AppDbContext>(
                opt => opt.UseNpgsql(Configuration.GetConnectionString("AppDbContext")).UseSnakeCaseNamingConvention());

            services.AddScoped<ILandingPointRepository, LandingPointRepository>();
            services.AddScoped<ILandingPointService, LandingPointService>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ModelToResourceProfile()); 
                mc.AddProfile(new ResourceToModelProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped<IUnitOfWork, UnitOfWork>();

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
