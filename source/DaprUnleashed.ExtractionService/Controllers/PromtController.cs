using DaprUnleashed.DomainModel;
using DaprUnleashed.ExtractionService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DaprUnleashed.ExtractionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromtController : ControllerBase
    {
        private readonly IExtractionService _extractionService;

        public PromtController(IExtractionService extractionService)
        {
            _extractionService = extractionService;
        }

        [Dapr.Topic("pubsub", "extract")]
        [HttpPost("extractreceived")]
        public async Task<IActionResult> PromtReceived(QueueRequest queueRequest)
        {
            try
            {
                await _extractionService.ExtractAsync(queueRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
