using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FancyMicroservice.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static void MapHealthChecks(this IApplicationBuilder app)
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

                    endpoints.MapHealthChecks("/ready", new HealthCheckOptions()
                    {
                        AllowCachingResponses = false,
                        ResponseWriter = WriteHealthCheckResponse,
                        //To assign particular HealthCheck class to url path we have to use filtering tags:
                        Predicate = (check) => check.Tags.Contains("readiness")
                    });

                    endpoints.MapHealthChecks("/live", new HealthCheckOptions()
                    {
                        AllowCachingResponses = false,
                        ResponseWriter = WriteHealthCheckResponse,
                        //To assign particular HealthCheck class to url path we have to use filtering tags:
                        Predicate = (check) => check.Tags.Contains("liveness")
                    });

                    // call GET http://localhost:[PORT]/health/ready to see status
                    // call GET http://localhost:[PORT]/health/live to see status
                });
            });
        }
    }
}
