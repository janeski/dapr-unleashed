namespace DaprUnleashed.ExtractionService.Services.Interfaces
{
    public interface IExtractionService
    {
        Task ExtractAsync(string queueMessage);
    }
}
