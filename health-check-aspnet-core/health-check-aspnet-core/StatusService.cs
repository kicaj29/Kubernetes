using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace health_check_aspnet_core
{
    public class StatusService
    {
        public HealthStatus HealthStatus { get; set; } = HealthStatus.Healthy;
    }


}
