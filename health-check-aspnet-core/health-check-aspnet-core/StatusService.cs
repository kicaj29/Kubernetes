using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace health_check_aspnet_core
{
    /// <summary>
    /// This service is used to emulate the status.
    /// </summary>
    public class StatusService
    {
        public HealthStatus HealthStatusStartup { get; set; } = HealthStatus.Unhealthy;
        public HealthStatus HealthStatusLiveness { get; set; } = HealthStatus.Unhealthy;

        public HealthStatus HealthStatusReadiness { get; set; } = HealthStatus.Unhealthy;
    }


}
