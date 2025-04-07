using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skaters.CustomActionFilters;
using Skaters.Models.DTO.CartProductDto;
using Skaters.Repositories.CartProductRepositories;
using System.Security.Claims;


namespace Skaters.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartProductController : ControllerBase
    {
        private readonly ICartProductRepository _cartProductRepository;

        public CartProductController(ICartProductRepository cartProductRepository)
        {
            _cartProductRepository = cartProductRepository;
        }

        [HttpPost]
        [ValidateModel]
       [Authorize(Roles ="Customer")]
        [Route("Products")]
        public async Task<IActionResult> CreateCartProducts([FromBody]List<AddCartProductDto> addrequest)
        {
            var userId = GetUserId();
            var cartProductDto=  await _cartProductRepository.AddAsync(addrequest,userId);
            if (cartProductDto == null) BadRequest();
            return Ok(cartProductDto);

        }
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateCartProduct([FromBody] AddorUpdateCartProductRequestDto addrequest)
        {
            
            var cartProductDto = await _cartProductRepository.AddOne(addrequest);
            if (cartProductDto == null) BadRequest();
            return Ok(cartProductDto);

        }
        [HttpGet]
        [Authorize(Roles ="Customer")]
        public async Task<IActionResult> GetCartProducts()
        { 
            var cartProducts= await _cartProductRepository.GetAllAsync();
            if (cartProducts == null) NotFound();
            return Ok(cartProducts);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        
        public async Task<IActionResult> GetCartProduct([FromRoute] Guid id)
        { 
            var cartProduct=await _cartProductRepository.GetAsync(id);
            if (cartProduct == null) BadRequest();    
            return Ok(cartProduct);
        }

        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> DeleteCartProduct([FromRoute] Guid id)
        {
            try
            {
                var cartProduct = await _cartProductRepository.DeleteAsync(id,GetUserId());
                if (cartProduct == null) BadRequest();
                return Ok(cartProduct);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
            
        }

        [HttpGet]
        [Route("cart={cartId:Guid}")]
        [Authorize(Roles ="Customer")]
        public async Task<IActionResult> GetProductsByCartId([FromRoute] Guid cartId)
        {
            var products = await _cartProductRepository.GetProductsByCart(cartId);
            if (products == null) NotFound();   
            return Ok(products);    
        }

        [HttpGet]
        [Route("cartproducts")]
        [Authorize(Roles ="Customer")]
        public async Task<IActionResult> GetCartProductsByUserId([FromQuery]string? status) {
            string userId = GetUserId();
            var result =await _cartProductRepository.GetCartproductsByUser(userId,status);
            if (result == null) NotFound("Something went wrong!!!");
            return Ok(result);
        }


        [HttpGet]
        [Route("cartproduct={cartId:Guid}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetCartProductsById([FromRoute]Guid cartId)
        {
            var result = await _cartProductRepository.GetCartproductsByCartId(cartId);
            if (result == null) NotFound("Something went wrong!!!");
            return Ok(result);
        }
        private string GetUserId()
        {
            var identity = User.FindFirstValue(ClaimTypes.UserData);
            return identity;
        }

    }
}
