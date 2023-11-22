using DaprUnleashed.DomainModel.Interfaces;
using Microsoft.Azure.Cosmos;

namespace DaprUnleashed.DomainModel.Implementations
{
    public class StorageService : IStorageService
    {
        private readonly Container _container;
        public StorageService(Container container)
        {
            _container = container;
        }

        public async Task<Promt> GetByIdAsync(Guid id, string type)
        {
            var response = await _container.ReadItemAsync<Promt>(id.ToString(), new PartitionKey(type));
            return response.Resource;
        }
        public async Task<Promt> InsertAsync(Promt promt)
        {
            var response = await _container.CreateItemAsync(promt, new PartitionKey(promt.Type));
            return response.Resource;
        }
        public async Task<Promt> UpdateAsync(Guid id, Promt promt)
        {
            var response = await _container.ReplaceItemAsync(promt, id.ToString(), new PartitionKey(promt.Type));
            return response.Resource;
        }
    }
}
