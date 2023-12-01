using DaprUnleashed.DomainModel;

namespace DaprUnleashed.TransformationService.Services.Interfaces
{
    public interface ITransformationService
    {
        Task TransformAsync(QueueRequest queueRequest);
    }
}
