using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;

namespace FancyMicroservice.HealthChecks
{
    public class ReadinessHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
                HealthCheckContext context,
                CancellationToken cancellationToken = default(CancellationToken))
        {
            Debug.WriteLine("Executing ReadinessHealthCheck");
            return Task.FromResult(HealthCheckResult.Healthy("Reported Healthy"));
        }
    }
}
