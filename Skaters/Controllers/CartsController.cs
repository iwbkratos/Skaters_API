using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Skaters.CustomActionFilters;
using Skaters.Models.DTO.CartDTOs;
using Skaters.Repositories.CartRepositories;
using System.Security.Claims;

namespace Skaters.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartsController(ICartRepository cartRepository) 
        {
            _cartRepository = cartRepository;
        }

        [HttpGet]
        [Route("newcart")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateCart()
        {
            string userId = GetUserId();
           var cartDto=await _cartRepository.getCart(userId);
            if (cartDto == null) return BadRequest();
            return Ok(cartDto);
        }
       
        

        [HttpGet]
        [Authorize(Roles ="Customer")]
        public async Task<IActionResult> GetCarts()
        { 
           var cartDto=await _cartRepository.GetAllAsync();
            if (cartDto == null) return BadRequest();   
            return Ok(cartDto); 
        }

        [HttpPut]
        [Route("update/{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles ="Customer")]
        public async Task<IActionResult> UpdateCartStatus([FromRoute]Guid id,[FromBody]AddCartRequestDto updateCartRequestDto)
        {
              var cartDto= await _cartRepository.UpdateAsync(id, updateCartRequestDto);
               if(cartDto == null) return BadRequest(); 
               return Ok(cartDto);
              
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteCart([FromRoute]Guid id)
        { 
              var cartDto=await _cartRepository.DeleteAsync(id);
            if (cartDto == null) return NotFound();
            return Ok(cartDto); 
        }

         private string GetUserId()
        {
            var identity = User.FindFirstValue(ClaimTypes.UserData);
            return identity;
        }

        
    }


}
