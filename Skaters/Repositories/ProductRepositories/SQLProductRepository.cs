using Microsoft.EntityFrameworkCore;
using Skaters.Data;
using Skaters.Domain.Model;
using System.Security.Claims;

namespace Skaters.Repositories.ProductRepositories
{
    public class SQLProductRepository : IProductRepository
    {
        private readonly SkatersAuthDbContext dbContext;
     

        public SQLProductRepository(SkatersAuthDbContext dbContext)
        {
            this.dbContext = dbContext;
          
        }
        public async Task<Product> CreateAsync(Product product,string userId)
        {
            product.UserId = userId;

            var store=await dbContext.Store.FirstOrDefaultAsync(x => x.UserId == userId);

            await dbContext.Products.AddAsync(product);
            await dbContext.SaveChangesAsync();
            return product;

        }

        public async Task<Product?> DeleteAsync(Guid id)
        {
            var ProductModelDomain = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (ProductModelDomain == null)
            {
                return null;
            }
            dbContext.Products.Remove(ProductModelDomain);
            await dbContext.SaveChangesAsync();
            return ProductModelDomain;
        }

        public async Task<List<Product>> getAllByStoreIdAsync(Guid userId)
        {
            var store = dbContext.Products.AsQueryable();
            var products = store.Where(x => x.StoreId == userId);
            return await products.ToListAsync();
        }
        public async Task<List<Product>> getAllAsync(string? filterOn, string? category, int pageNumber, int pageSize)
        {
            var products = dbContext.Products.AsQueryable();
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(category) == false)
            {
                if (filterOn.Equals("Category", StringComparison.OrdinalIgnoreCase))
                {
                    products = products.Where(x => x.Category.Contains(category));
                }
            }
            var skipResult = (pageNumber - 1) * pageSize;
            return await products.Skip(skipResult).Take(pageSize).ToListAsync();

        }

        public async Task<Product> getAsync(Guid id)
        {
            var ProductModelDomain = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

            return ProductModelDomain;
        }

        public async Task<Product> UpdateAsync(Guid id, Product product)
        {
            var ProductModelDomain = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (ProductModelDomain == null)
            {
                return null;
            }
            
            ProductModelDomain.Name = product.Name;
            ProductModelDomain.Description = product.Description;
            ProductModelDomain.Category = product.Category;
            ProductModelDomain.Price = product.Price;
            ProductModelDomain.ImageUrl = product.ImageUrl;
            await dbContext.SaveChangesAsync();
            return ProductModelDomain;
        }


    }
}
