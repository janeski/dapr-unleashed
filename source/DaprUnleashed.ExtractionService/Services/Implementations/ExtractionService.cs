using Dapr.Client;
using DaprUnleashed.DomainModel;
using DaprUnleashed.ExtractionService.Services.Interfaces;

namespace DaprUnleashed.ExtractionService.Services.Implementations
{
    public class ExtractionService : IExtractionService
    {
        private readonly DaprClient _daprClient;

        public ExtractionService(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        public async Task ExtractAsync(QueueRequest queueRequest)
        {
            var metadata = new Dictionary<string, string> { { "partitionKey", queueRequest.Type } };

            var promt = await _daprClient.GetStateAsync<Promt>("promtstore", queueRequest.Id.ToString(), metadata: metadata);
            promt.StateTransitions.Add(new StateTransition { State = "7. Start to extract", DateTime = DateTime.UtcNow });

            //call Azure OpenAI Services
            await Task.Delay(2000);

            promt.StateTransitions.Add(new StateTransition { State = "8. Extraction finished", DateTime = DateTime.UtcNow });
            promt.StateTransitions.Add(new StateTransition() { State = "9. Save to storage", DateTime = DateTime.UtcNow });
            await _daprClient.SaveStateAsync<Promt>("promtstore", promt.id.ToString(), promt, metadata: metadata);
        }
    }
}
