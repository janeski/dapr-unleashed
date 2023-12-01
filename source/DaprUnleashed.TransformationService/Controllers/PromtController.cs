using DaprUnleashed.DomainModel;
using DaprUnleashed.TransformationService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DaprUnleashed.TransformationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromtController : ControllerBase
    {
        private readonly ITransformationService _transformationService;

        public PromtController(ITransformationService transformationService)
        {
            _transformationService = transformationService;
        }

        [Dapr.Topic("pubsub", "transform")]
        [HttpPost("transformreceived")]
        public async Task<IActionResult> PromtReceived(QueueRequest queueRequest)
        {
            try
            {
                await _transformationService.TransformAsync(queueRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
