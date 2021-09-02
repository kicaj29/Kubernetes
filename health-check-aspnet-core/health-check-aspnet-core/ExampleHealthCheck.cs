using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace health_check_aspnet_core
{
    public class ExampleHealthCheck : IHealthCheck
    {
        private StatusService _statusSvc;

        public ExampleHealthCheck(StatusService statusSvc)
        {
            this._statusSvc = statusSvc;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
                HealthCheckContext context,
                CancellationToken cancellationToken = default(CancellationToken))
        {

            switch(this._statusSvc.HealthStatus)
            {
                case HealthStatus.Unhealthy:
                    return Task.FromResult(HealthCheckResult.Unhealthy("Reported Unhealthy"));
                case HealthStatus.Degraded:
                    return Task.FromResult(HealthCheckResult.Degraded("Reported Degraded"));
                default:
                    return Task.FromResult(HealthCheckResult.Healthy("Reported Healthy"));
            }         
        }
    }
}
