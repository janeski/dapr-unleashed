using DaprUnleashed.API.Services.Interfaces;
using DaprUnleashed.DomainModel;
using Microsoft.AspNetCore.Mvc;

namespace DaprUnleashed.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromtController : ControllerBase
    {

        private readonly ILogger<PromtController> _logger;
        private readonly IPromtService _promtService;

        public PromtController(ILogger<PromtController> logger, IPromtService promtService)
        {
            _logger = logger;
            _promtService = promtService;
        }

        [HttpPost]
        public async Task<ActionResult> PostPromt([FromBody] Promt promt)
        {
            try
            {
                promt.partitionKey = promt.Type;
                await _promtService.ProcessAsync(promt);
                return CreatedAtAction(nameof(PostPromt), new { promt.id }, promt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing promt");
                throw;
            }
        }

        [HttpGet]
        public async Task<ActionResult> Get(Guid id, string type)
        {
            try
            {
                var result = await _promtService.GetPromtAsync(id, type);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting promt");
                throw;
            }
        }
    }
}