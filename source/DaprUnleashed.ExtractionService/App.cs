using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Security.KeyVault.Secrets;
using DaprUnleashed.DomainModel.Implementations;
using DaprUnleashed.ExtractionService.Services.Interfaces;
using Microsoft.Azure.Cosmos;

namespace DaprUnleashed.ExtractionService
{
    public class App
    {
        private ServiceBusClient? _extractServiceBusClient;
        private ServiceBusProcessor? _processor;
        private IExtractionService? _extractionService;

        public async Task Run(CancellationToken cancellationToken)
        {
            try
            {
                await InitializeAsync();

                Start();
                Console.WriteLine("Extraction started...");
                await Task.Delay(Timeout.Infinite, cancellationToken);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Transformation stopped.");
                Stop();
            }
        }

        private async Task InitializeAsync()
        {
            string keyVaultName = Environment.GetEnvironmentVariable("DaprUnleashedKeyVault");
            var keyVaultUri = new Uri(keyVaultName);
            var keyVaultClient = new SecretClient(keyVaultUri, new DefaultAzureCredential());
            KeyVaultSecret secretKey = keyVaultClient.GetSecret("dapr-unleashed-sb-dev");
            var serviceBusClientConnectionString = secretKey.Value;

            secretKey = keyVaultClient.GetSecret("dapr-unleashed-cosmosdb-dev");
            var cosmosDbConnectionString = secretKey.Value;
            _extractServiceBusClient = new ServiceBusClient(serviceBusClientConnectionString);

            _processor = _extractServiceBusClient.CreateProcessor("extract", new ServiceBusProcessorOptions());

            var _cosmosClient = new CosmosClient(cosmosDbConnectionString);
            string databaseName = "dapr-unleashed-cosmosdb-dev";
            string containerName = "promts";
            var database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName);
            await database.Database.CreateContainerIfNotExistsAsync(containerName, "/Type");
            var _container = _cosmosClient.GetContainer(databaseName, containerName);
            var storageService = new StorageService(_container);

            _extractionService = new Services.Implementations.ExtractionService(storageService);
            Console.WriteLine("Initialization finished");
        }

        private void Start()
        {
            _processor.ProcessMessageAsync += MessageHandler;

            _processor.ProcessErrorAsync += ErrorHandler;

            _processor.StartProcessingAsync().Wait();
            Console.WriteLine("Waiting for messages...");
        }

        private void Stop()
        {
            _processor.StopProcessingAsync().Wait();
        }
        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            await _extractionService.ExtractAsync(body);
            await args.CompleteMessageAsync(args.Message);
        }

        private static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            return Task.CompletedTask;
        }
    }
}
