using Skaters.Domain.Model;
using Skaters.Models.Domain;
using Skaters.Models.DTO.CartProductDto;

namespace Skaters.Repositories.CartProductRepositories
{
    public interface ICartProductRepository
    {
        public  Task<CartProductDto> AddOne(AddorUpdateCartProductRequestDto addRequest);
        public Task<List<CartProductDto>> AddAsync(List<AddCartProductDto> addRequest,string userId);
        public Task<List<CartProduct>> GetAllAsync();
        public Task<CartProductDto?> GetAsync(Guid id);
        public  Task<CartProduct> DeleteAsync(Guid id);
        public Task<List<Product>> GetProductsByCart(Guid id);
        public Task<List<OrdersResponse>> GetCartproductsByUser(string userId,string? status);
        public Task<OrdersResponse> GetCartproductsByCartId(Guid id);
    }
}
