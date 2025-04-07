using Skaters.Domain.Model;

namespace Skaters.Repositories.ProductRepositories
{
    public interface IProductRepository
    {
        public Task<List<Product>> getAllAsync(string? filterOn, string? filterQuery, int pageNumber, int pageSize);
        public Task<List<Product>> getAllByStoreIdAsync(Guid userId);
        public Task<Product> getAsync(Guid id);
        public Task<Product> CreateAsync(Product product,string userId);
        public Task<Product> UpdateAsync(Guid id, Product product);
        public Task<Product?> DeleteAsync(Guid id);
        Task<int> CategoryCount(string category);
    }
}
