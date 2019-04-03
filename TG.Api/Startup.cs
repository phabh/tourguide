using Lime.Protocol;
using Lime.Protocol.Serialization.Newtonsoft;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestEase;
using TG.Api.Interfaces;
using TG.Api.Interfaces.Clients;
using TG.Api.Services;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;

namespace TG.Api
{
    public class Startup
    {
        private const string HealthEndpoint = "/health";
        private string AppName { get; } = Assembly.GetExecutingAssembly().GetName().Name;
        private const string CurrentVersion = "v1";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddHealthChecks();

            services
                .AddSingleton(RestClient.For<IGoogleMapsClient>("https://maps.googleapis.com/maps/api/"))
                .AddSingleton<IMapsService, GoogleMapsService>();

            services
                .AddMvc()
                .AddJsonOptions(o => JsonNetSerializer.Settings.Converters.ForEach(c => o.SerializerSettings.Converters.Add(c)))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Tourguide API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app
                    .UseHsts()
                    .UseHttpsRedirection();
            }

            app.UseHealthChecks(HealthEndpoint)
                .UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = string.Empty;
                c.SwaggerEndpoint("./swagger/v1/swagger.json", $"{AppName} {CurrentVersion}");
            });
        }
    }
}
