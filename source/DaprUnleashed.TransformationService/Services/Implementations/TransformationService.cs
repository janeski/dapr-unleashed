using Dapr.Client;
using DaprUnleashed.DomainModel;
using DaprUnleashed.TransformationService.Services.Interfaces;

namespace DaprUnleashed.TransformationService.Services.Implementations
{
    public class TransformationService : ITransformationService
    {
        private readonly DaprClient _daprClient;
        public TransformationService(DaprClient daprClient)
        {
            _daprClient = daprClient;
        }

        public async Task TransformAsync(QueueRequest queueRequest)
        {
            var metadata = new Dictionary<string, string> { { "partitionKey", queueRequest.Type } };

            var promt = await _daprClient.GetStateAsync<Promt>("promtstore", queueRequest.Id.ToString(),ConsistencyMode.Eventual,metadata);
            promt.StateTransitions.Add(new StateTransition { State = "3. Start to transform", DateTime = DateTime.UtcNow  });

            //call Azure Cognitive Services
            await Task.Delay(2000);

            promt.StateTransitions.Add(new StateTransition { DateTime = DateTime.UtcNow, State = "4. Transform finished" });
            promt.StateTransitions.Add(new StateTransition() { State = "5. Save to storage", DateTime = DateTime.UtcNow });
            promt.StateTransitions.Add(new StateTransition() { State = "6. Send to extract", DateTime = DateTime.UtcNow });
            await _daprClient.SaveStateAsync("promtstore", promt.id.ToString(), promt, metadata: metadata);
            await _daprClient.PublishEventAsync("pubsub", "extract", queueRequest);

        }
    }
}
