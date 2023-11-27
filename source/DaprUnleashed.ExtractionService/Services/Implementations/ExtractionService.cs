using System.Text.Json;
using DaprUnleashed.DomainModel;
using DaprUnleashed.DomainModel.Interfaces;
using DaprUnleashed.ExtractionService.Services.Interfaces;

namespace DaprUnleashed.ExtractionService.Services.Implementations
{
    public class ExtractionService : IExtractionService
    {
        private readonly IStorageService _storageService;

        public ExtractionService(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public async Task ExtractAsync(string queueMessage)
        {
            var messageFromQueue = JsonSerializer.Deserialize<QueueRequest>(queueMessage);

            if (messageFromQueue == null)
            {
                return;
            }

            var promt = await _storageService.GetByIdAsync(messageFromQueue.Id, messageFromQueue.Type);
            promt.StateTransitions.Add(new StateTransition { DateTime = DateTime.UtcNow, State = "7. Start to extract" });

            //call Azure OpenAI Services
            await Task.Delay(2000);

            promt.StateTransitions.Add(new StateTransition { DateTime = DateTime.UtcNow, State = "8. Extraction finished" });
            promt.StateTransitions.Add(new StateTransition() { State = "9. Save to storage", DateTime = DateTime.UtcNow });
            await _storageService.UpdateAsync(promt.id, promt);
        }
    }
}
