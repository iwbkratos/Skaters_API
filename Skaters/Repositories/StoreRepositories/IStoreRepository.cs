using Skaters.Domain.Model;
using Skaters.Models.DTO;

namespace Skaters.Repositories.StoreRepositories
{
    public interface IStoreRepository
    {
        public Task<List<Store>> getAllAsync();
        public Task<Store> getAsync(Guid id);
        public Task<List<Store>> getByUserId(string id);
        public Task<Store> CreateAsync(Store store,string userId);
        public Task<Store?> UpdateAsync(Guid id, Store store);
        public Task<Store?> DeleteAsync(Guid id);
    }
}
