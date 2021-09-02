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
    public class StatusManagementStarupController : ControllerBase
    {

        private StatusService _statusSvc;

        public StatusManagementStarupController(StatusService statusSvc)
        {
            this._statusSvc = statusSvc;
        }

        [HttpPost]
        public void SetStatus(HealthStatus status)
        {
            this._statusSvc.HealthStatusStartup = status;
        }
    }
}
