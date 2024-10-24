using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Skaters.Data;
using Skaters.Domain.Model;
using Skaters.Models.CustomClass;
using Skaters.Models.DTO;
using System.Security.Claims;

namespace Skaters.Repositories.StoreRepositories
{
    public class SQLStoreRepository : IStoreRepository
    {
        private readonly SkatersAuthDbContext _dbContext;
        private readonly ClaimsPrincipal user;

        public SQLStoreRepository(SkatersAuthDbContext dbContext)
        {
            _dbContext = dbContext;
            
        }
       
        public async Task<Store> CreateAsync(Store store, string userId)
        {
            store.UserId = userId;
            await _dbContext.Store.AddAsync(store);
            await _dbContext.SaveChangesAsync();
            return store;
        }


        public async Task<List<Store>> getAllAsync()
        {
            return await _dbContext.Store.ToListAsync();

        }

        public async Task<Store> getAsync(Guid id)
        {
            var store = await _dbContext.Store.FirstOrDefaultAsync(x => x.Id == id);

            return store;
        }

        public async Task<Store?> UpdateAsync(Guid id, Store store)
        {
            var storeModelDomain = await _dbContext.Store.FirstOrDefaultAsync(x => x.Id == id);
            if (storeModelDomain == null)
            {
                return null;
            }
            storeModelDomain.Name = store.Name;
            storeModelDomain.Description = store.Description;
            await _dbContext.SaveChangesAsync();
            return storeModelDomain;
        }
        public async Task<Store?> DeleteAsync(Guid id)
        {
            var storeModelDomain = await _dbContext.Store.FirstOrDefaultAsync(x => x.Id == id);
            if (storeModelDomain == null) return null;
            _dbContext.Store.Remove(storeModelDomain);
            await _dbContext.SaveChangesAsync();
            return storeModelDomain;
        }

        public async Task<List<Store>> getByUserId(string userId)
        {
            var storeModelDomain = _dbContext.Store.AsQueryable();
            storeModelDomain = storeModelDomain.Where(x => x.UserId == userId);
            if (storeModelDomain == null) return null;
            return await storeModelDomain.ToListAsync();
          
        }
    }
}
