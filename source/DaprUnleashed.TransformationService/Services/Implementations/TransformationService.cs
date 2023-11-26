using System.Text.Json;
using DaprUnleashed.DomainModel;
using DaprUnleashed.DomainModel.Interfaces;
using DaprUnleashed.TransformationService.Services.Interfaces;

namespace DaprUnleashed.TransformationService.Services.Implementations
{
    public class TransformationService : ITransformationService
    {
        private readonly IStorageService _storageService;
        private readonly IQueueService _queueService;

        public TransformationService(IStorageService storageService, IQueueService queueService)
        {
            _storageService = storageService;
            _queueService = queueService;
        }

        public async Task TransformAsync(string queueMessage)
        {
            var messageFromQueue = JsonSerializer.Deserialize<QueueRequest>(queueMessage);

            if (messageFromQueue == null)
            {
                return;
            }

            var promt = await _storageService.GetByIdAsync(messageFromQueue.Id, messageFromQueue.Type);
            promt.StateTransitions.Add(new StateTransition { DateTime = DateTime.UtcNow, State = "3. Start to transform" });

            //call Azure Cognitive Services
            await Task.Delay(2000);

            promt.StateTransitions.Add(new StateTransition { DateTime = DateTime.UtcNow, State = "4. Transform finished" });
            promt.StateTransitions.Add(new StateTransition() { State = "5. Send to extract", DateTime = DateTime.UtcNow });
            promt.StateTransitions.Add(new StateTransition() { State = "6. Save to storage", DateTime = DateTime.UtcNow });
            await _storageService.UpdateAsync(promt.id, promt);
            await _queueService.SendAsync(queueMessage);
        }
    }
}
