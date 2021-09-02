using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace health_check_aspnet_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusManagementLivenessController : ControllerBase
    {

        private StatusService _statusSvc;

        public StatusManagementLivenessController(StatusService statusSvc)
        {
            this._statusSvc = statusSvc;
        }

        [HttpPost]
        public void SetStatus(HealthStatus status)
        {
            this._statusSvc.HealthStatusLiveness = status;
        }
    }
}
