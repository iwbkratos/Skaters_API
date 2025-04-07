using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skaters.CustomActionFilters;
using Skaters.Domain.Model;
using Skaters.Models.CustomClass;
using Skaters.Models.DTO.ProductDTOs;
using Skaters.Repositories.ProductRepositories;
using System.Security.Claims;

namespace Skaters.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IProductRepository productRepository;

        public ProductsController(IMapper mapper, IProductRepository productRepository)
        {
            this.mapper = mapper;
            this.productRepository = productRepository;
        }

        [HttpGet]
        [Route("products")]
        public async Task<IActionResult> GetProducts([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
                                                 [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 8)
        {
            var productModelDomain = await productRepository.getAllAsync(filterOn, filterQuery, pageNumber, pageSize);
            if (productModelDomain == null)
            {
                NotFound();
            }
            // var productDto = mapper.Map<ProductDto>(productModelDomain);
            return Ok(productModelDomain);
        }

        [HttpGet]
        [Route("store/{storeId:Guid}")]
       
        public async Task<IActionResult> GetProductsByStoreId([FromRoute] Guid storeId)
        {
            var productModelDomain = await productRepository.getAllByStoreIdAsync(storeId);
            if (productModelDomain == null)
            {
                NotFound();
            }
            // var productDto = mapper.Map<ProductDto>(productModelDomain);
            return Ok(productModelDomain);
        }


        [HttpGet]
        [Route("{productid:Guid}")]       
        public async Task<IActionResult> GetProduct([FromRoute] Guid productid)
        {
           var productModelDomain=  await productRepository.getAsync(productid);
            if(productModelDomain==null) NotFound();
            var productDto = mapper.Map<ProductDto>(productModelDomain);
            return Ok(productDto);
        }

        [HttpPost]
        
        [Authorize(Roles =("Seller"))]
        public async Task<IActionResult> AddProduct([FromBody] AddorProductRequestDto addProductRequestDto)
        {
            string userId = GetUserId();
            var productModelDomain = mapper.Map<Product>(addProductRequestDto);
                productModelDomain= await productRepository.CreateAsync(productModelDomain,userId);
            if (productModelDomain == null)
            {
                BadRequest();
            }
            var productDto = mapper.Map<ProductDto>(productModelDomain);
            return Ok(new HttpResponse<ProductDto>(StatusCodes.Status200OK,"item added to cart",productDto));
        }

        [HttpPut]
        [Route("update/{id:Guid}")]
       [Authorize(Roles="Seller")]
        public async Task<IActionResult> UpdateProduct([FromRoute]Guid id,[FromBody] AddorProductRequestDto updateProductRequestDto)
        {
            var productModelDomainModel = mapper.Map<Product>(updateProductRequestDto);
            productModelDomainModel = await productRepository.UpdateAsync(id,productModelDomainModel);
            if (productModelDomainModel == null) 
            { NotFound(); }

            var prodcutDto = mapper.Map<ProductDto>(productModelDomainModel);
            return Ok(prodcutDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles ="Seller")]
        public async Task<IActionResult> DeleteProduct([FromRoute]Guid id)
        { 
           var productDomainModel= await productRepository.DeleteAsync(id);
            if (productDomainModel == null)
            {
                return BadRequest();
            }
            var productDto = mapper.Map<ProductDto>(productDomainModel);
            return Ok(productDto);
        }

        [HttpGet("categorycount")]
       // [Authorize]
        public async Task<IActionResult> GetProductOfCategoryCount([FromQuery]string category)
        {
            int count = await productRepository.CategoryCount(category);
            return Ok(count);
        }
        private string GetUserId()
        {
            var identity = User.FindFirstValue(ClaimTypes.UserData);
            return identity;
        }
    }

}
