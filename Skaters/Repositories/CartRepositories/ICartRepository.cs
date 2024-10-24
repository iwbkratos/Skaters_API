using Skaters.Models.Domain;
using Skaters.Models.DTO.CartDTOs;

namespace Skaters.Repositories.CartRepositories
{
    public interface ICartRepository
    {

        public Task<CartDto> CreateAsync(AddCartRequestDto addCartRequestDto, string userID);   
        public Task<List<Cart>> GetAllAsync();
        public Task<CartDto> UpdateAsync(Guid id,AddCartRequestDto updateCartRequestDto);
        public Task<CartDto?> DeleteAsync(Guid id);
        public  Task<CartDto> getCart(string userId);

    }
}
