using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace health_check_aspnet_core
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "health_check_aspnet_core", Version = "v1" });
            });

            services.AddSingleton<StatusService>();


            //services.AddHealthChecks();
            services.AddHealthChecks()
                .AddCheck<ExampleHealthCheck>(
                    "example health check"                    
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            MapHealthChecks(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "health_check_aspnet_core v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void MapHealthChecks(IApplicationBuilder app)
        {
            app.Map("/health", healthApp =>
            {
                healthApp.UseRouting();
                healthApp.UseEndpoints(endpoints =>
                {
                    Task WriteHealthCheckResponse(HttpContext context, HealthReport result)
                    {
                        context.Response.ContentType = "text/plain";
                        return context.Response.WriteAsync(result.Status.ToString());
                    };

                    HealthCheckOptions healthCheckOptions = new HealthCheckOptions()
                    {
                        AllowCachingResponses = false,
                        ResponseWriter = WriteHealthCheckResponse
                    };

                    endpoints.MapHealthChecks("/ready", healthCheckOptions);
                    endpoints.MapHealthChecks("/live", healthCheckOptions);
                    endpoints.MapHealthChecks("/startup", healthCheckOptions);

                    // call GET https://localhost:5001/health/ready to see status
                    // call GET https://localhost:5001/health/live to see status
                    // call GET https://localhost:5001/health/startup to see status
                });
            });
        }
    }
}
