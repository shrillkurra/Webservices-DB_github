using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebServices2DB_PoC.Entities;
using Microsoft.EntityFrameworkCore;
using WebServices2DB_PoC.Services;

namespace WebServices2DB_PoC
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
            services.AddMvc();

            var connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=PlockerDB;Trusted_Connection=True";
            services.AddDbContext<LandingPageContext>(op => op.UseSqlServer(connectionString));
            services.AddScoped<ILandingPageRepository, LandingPageRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,
            LandingPageContext landingPageContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePages();
            // V2
            // Add 'Automapper' through NuGet
            // Associate mappings between 'Entities' and 'Models'
            AutoMapper.Mapper.Initialize(cfg => 
            {
                cfg.CreateMap<Entities.LandingPageSummary, Models.LandingPageSimple>();
                cfg.CreateMap<Entities.LandingPageSummary, Models.LandingPageSummaryDto>();
                cfg.CreateMap<Entities.LandingPageDetail, Models.LandingPageDetailDto>();

                cfg.CreateMap<Models.LandingPageDetailCreationDto, Entities.LandingPageDetail>();
                cfg.CreateMap<Entities.LandingPageDetail, Models.LandingPageDetailCreationDto>();
                
            });
            app.UseMvc();

            landingPageContext.EnsureSeedDataForContext();
        }
    }
}
