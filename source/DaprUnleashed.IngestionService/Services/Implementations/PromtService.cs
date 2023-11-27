using System.Text.Json;
using DaprUnleashed.API.Services.Interfaces;
using DaprUnleashed.DomainModel;
using DaprUnleashed.DomainModel.Interfaces;

namespace DaprUnleashed.API.Services.Implementations
{
    public class PromtService : IPromtService
    {
        private readonly IStorageService _storageService;
        private readonly IQueueService _queueService;

        public PromtService(IStorageService storageService, IQueueService queueService)
        {
            _storageService = storageService;
            _queueService = queueService;
        }

        public Task<Promt> GetPromtAsync(Guid id, string type)
        {
            return _storageService.GetByIdAsync(id, type);
        }

        public async Task ProcessAsync(Promt promt)
        {
            var queueRequest = new QueueRequest() { Id = promt.id, Type = promt.Type };
            var queueSerialized = JsonSerializer.Serialize(queueRequest);
            promt.StateTransitions.Add(new StateTransition() { State = "1. Send to transform", DateTime = DateTime.UtcNow });
            promt.StateTransitions.Add(new StateTransition() { State = "2. Save to storage", DateTime = DateTime.UtcNow });
            await _storageService.InsertAsync(promt);
            await _queueService.SendAsync(queueSerialized);
        }
    }
}
