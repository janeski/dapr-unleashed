namespace DaprUnleashed.TransformationService.Services.Interfaces
{
    public interface ITransformationService
    {
        Task TransformAsync(string queueMessage);
    }
}
