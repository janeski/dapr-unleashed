using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Security.KeyVault.Secrets;
using DaprUnleashed.API;
using DaprUnleashed.API.Services.Implementations;
using DaprUnleashed.API.Services.Interfaces;
using DaprUnleashed.DomainModel.Implementations;
using DaprUnleashed.DomainModel.Interfaces;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppSettings();
builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

RegisterQueueService(builder.Services);
RegisterStorageService(builder.Services).Wait();
builder.Services.AddSingleton<IPromtService, PromtService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static async Task RegisterStorageService(IServiceCollection services)
{
    string keyVaultName = Environment.GetEnvironmentVariable("DaprUnleashedKeyVault");
    var keyVaultUri = new Uri(keyVaultName);
    var keyVaultClient = new SecretClient(keyVaultUri, new DefaultAzureCredential());
    KeyVaultSecret secretKey = keyVaultClient.GetSecret("dapr-unleashed-cosmosdb-dev");
    var cosmosDbConnectionString = secretKey.Value;
    var _cosmosClient = new CosmosClient(cosmosDbConnectionString);
    string databaseName = "dapr-unleashed-cosmosdb-dev";
    string containerName = "promts";

    DatabaseResponse database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName);
    await database.Database.CreateContainerIfNotExistsAsync(containerName, "/partitionKey");

    var _container = _cosmosClient.GetContainer(databaseName, containerName);

    services.AddSingleton<IStorageService, StorageService>(provider =>
    {
        return new StorageService(_container);
    });
}

static void RegisterQueueService(IServiceCollection services)
{
    string keyVaultName = Environment.GetEnvironmentVariable("DaprUnleashedKeyVault");
    var keyVaultUri = new Uri(keyVaultName);
    var keyVaultClient = new SecretClient(keyVaultUri, new DefaultAzureCredential());
    KeyVaultSecret secretKey = keyVaultClient.GetSecret("dapr-unleashed-sb-dev");
    var serviceBusConnectionString = secretKey.Value;

    var serviceBusClient = new ServiceBusClient(serviceBusConnectionString);
    services.AddSingleton(serviceBusClient);

    services.AddSingleton<IQueueService, QueueService>(provider =>
    {
        return new QueueService(serviceBusClient, "transform");
    });
}