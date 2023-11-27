using DaprUnleashed.DomainModel;

namespace DaprUnleashed.API.Services.Interfaces
{
    public interface IPromtService
    {
        Task<Promt> GetPromtAsync(Guid id, string type);
        Task ProcessAsync(Promt promt);
    }
}