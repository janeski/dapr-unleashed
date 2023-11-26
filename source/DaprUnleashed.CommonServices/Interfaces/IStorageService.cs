namespace DaprUnleashed.DomainModel.Interfaces
{
    public interface IStorageService
    {
        Task<Promt> GetByIdAsync(Guid id, string type);
        Task<Promt> InsertAsync(Promt promt);
        Task<Promt> UpdateAsync(Guid id, Promt promt);
    }
}