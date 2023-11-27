using Azure.Messaging.ServiceBus;
using DaprUnleashed.DomainModel.Interfaces;

namespace DaprUnleashed.DomainModel.Implementations
{
    public class QueueService : IQueueService
    {
        private readonly ServiceBusSender _serviceSender;

        public QueueService(ServiceBusClient client, string queueName)
        {
            _serviceSender = client.CreateSender(queueName);
        }

        public async Task SendAsync(string promt)
        {
            ServiceBusMessage message = new(promt);
            await _serviceSender.SendMessageAsync(message);
        }

    }
}
