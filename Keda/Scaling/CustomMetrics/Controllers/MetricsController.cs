using CustomMetrics.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomMetrics.Controllers
{
    [ApiController]
    [Route("api/metrics")]
    [Produces("application/json")]
    public class MetricsController : ControllerBase
    {
        private ILogger _logger;
        private static int _currentWaitingSize = 2;

        public MetricsController(ILogger<MetricsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("mongo-connections")]
        public async Task<IActionResult> GetMongoConnections()
        {
            _logger.LogInformation($"{DateTime.Now} Reporting metric mongo-connections: {_currentWaitingSize}.");
            return Ok(new MongoConnectionsMetrics() { CurrentWaitingSize = _currentWaitingSize });
        }

        [HttpPut("mongo-connections")]
        public async Task<IActionResult> UpdateMongoConnections(int newValue)
        {
            _logger.LogInformation($"{DateTime.Now} Updating metric mongo-connections with value {newValue}.");
            _currentWaitingSize = newValue;
            return Ok(new MongoConnectionsMetrics() { CurrentWaitingSize = _currentWaitingSize });
        }
    }
}
