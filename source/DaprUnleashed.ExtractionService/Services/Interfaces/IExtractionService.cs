using DaprUnleashed.DomainModel;

namespace DaprUnleashed.ExtractionService.Services.Interfaces
{
    public interface IExtractionService
    {
        Task ExtractAsync(QueueRequest queueRequest);
    }
}
