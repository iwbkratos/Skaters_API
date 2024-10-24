using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skaters.CustomActionFilters;
using Skaters.Data;
using Skaters.Domain.Model;
using Skaters.Models.DTO.StoreDTOs;
using Skaters.Repositories.ProductRepositories;
using Skaters.Repositories.StoreRepositories;
using System.Security.Claims;

namespace Skaters.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
       
        private readonly IProductRepository productRepository;
        private readonly IStoreRepository storeRepository;
        private readonly IMapper mapper;

        public StoreController(IProductRepository productRepository,IStoreRepository storeRepository, IMapper mapper)
        {
            
            this.productRepository = productRepository;
            this.storeRepository = storeRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStores()
        {
            var storesModelDomain=await storeRepository.getAllAsync();

            if (storesModelDomain == null)
                return NotFound();
           // var stores = mapper.Map<List<StoreDto>>(storesModelDomain);
            return Ok(storesModelDomain);
        }
       
        [HttpGet]
        [Route("{id:Guid}")]
      //  [Authorize(Roles="Seller")]
        public async Task<IActionResult> GetStore(Guid id)
        {
           var storeModelDomain=await storeRepository.getAsync(id);
            if(storeModelDomain==null)return NotFound();
          var store= mapper.Map<StoreDto>(storeModelDomain);
            return Ok(store);
        }
    

        [HttpGet]
        [Route("store")]
     //   [Authorize(Roles ="Seller")]
        public async Task<IActionResult> GetStoreByUserId()
        {
            string userId = GetUserId();
            var store = await storeRepository.getByUserId(userId);
            if (store == null)
            {
                NotFound();
            }
            return Ok(store);
        
         }

        [HttpPost]
        [ValidateModel]
       
        public async Task<IActionResult> AddStore([FromBody]AddStoreRequestDto addStoreRequestDto) 
        {
            string userId=GetUserId();
            var storeModelDomain = mapper.Map<Store>(addStoreRequestDto);
            if (storeModelDomain == null)return NotFound();
            storeModelDomain = await storeRepository.CreateAsync(storeModelDomain, userId);
            var storeDto = mapper.Map<StoreDto>(storeModelDomain);
            return Ok(storeDto);    
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
      //  [Authorize(Roles ="Seller")]
        public async Task<IActionResult> UpdateStore([FromRoute] Guid id,[FromBody] UpdateStoreRequestDto updateStoreRequestDto)
        {
            var storeModelDomain = mapper.Map<Store>(updateStoreRequestDto);
            
            storeModelDomain=  await storeRepository.UpdateAsync(id,storeModelDomain);
            if (storeModelDomain == null)
            {
                return NotFound();
            }
            var storeDto = mapper.Map<StoreDto>(storeModelDomain);
            return Ok(storeDto);    
        }

        [HttpDelete]
        [Route("{id:Guid}")]
      //  [Authorize(Roles ="Seller")]
        public async Task<IActionResult> DeleteStore([FromRoute] Guid id)
        { 
          var storeModelDomain= await storeRepository.DeleteAsync(id);
            if(storeModelDomain==null)return NotFound();
            var storeDto = mapper.Map<StoreDto>(storeModelDomain);
            return Ok(storeDto);

        }

        private string GetUserId()
        {
            var identity = User.FindFirstValue(ClaimTypes.UserData);
            return identity;
        }
    }
}
