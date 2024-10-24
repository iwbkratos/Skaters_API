using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Skaters.Data;
using Skaters.Domain.Model;
using Skaters.Models.Domain;
using Skaters.Models.DTO.CartProductDto;
using Skaters.Repositories.CartRepositories;
using System.Collections.Generic;

namespace Skaters.Repositories.CartProductRepositories
{
    public class CartProductRepository : ICartProductRepository
    {
        private readonly SkatersAuthDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICartRepository _cartRepository;
        public CartProductRepository(SkatersAuthDbContext dbContext,IMapper mapper, ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<CartProductDto>> AddAsync(List<AddCartProductDto> addRequest, string userId)
        {
            
            var cartDto = await _cartRepository.getCart(userId);
            var addCartProduct = new List<AddorUpdateCartProductRequestDto>();
            var existingData= addRequest.ToList();
            foreach (var item in existingData) 
            {
               var obj=  new AddorUpdateCartProductRequestDto()
                {
                    CartId=cartDto.Id,
                    ProductId=item.ProductId,
                    Quantity=item.Quantity,
                };
                addCartProduct.Add(obj);
            }

            var cartproductModelDomain = _mapper.Map<List<CartProduct>>(addCartProduct);
            await _dbContext.CartProduct.AddRangeAsync(cartproductModelDomain);
            await _dbContext.SaveChangesAsync();
            var cartproductDto=_mapper.Map<List<CartProductDto>>(cartproductModelDomain);
            return cartproductDto;
        }
        public async Task<CartProductDto> AddOne(AddorUpdateCartProductRequestDto addRequest)
        {
            var cartproductModelDomain = _mapper.Map<CartProduct>(addRequest);
            await _dbContext.AddAsync(cartproductModelDomain);
            await _dbContext.SaveChangesAsync();
            var cartproductDto = _mapper.Map<CartProductDto>(cartproductModelDomain);
            return cartproductDto;
        }

        public async Task<List<CartProduct>> GetAllAsync()
        {
            return await _dbContext.CartProduct.ToListAsync();
        }

        public async Task<CartProductDto?> GetAsync(Guid id)
        {
            var CartProductModelDomain=await _dbContext.CartProduct.FirstOrDefaultAsync(x=>x.Id==id);
            if (CartProductModelDomain == null) return null;
            var CartProductsDto = _mapper.Map<CartProductDto>(CartProductModelDomain);
            return CartProductsDto;
        }

        public async Task<CartProduct> DeleteAsync(Guid id)
        {
            var cart = await _dbContext.Cart.FirstOrDefaultAsync(x => x.Status == "active");
            var CartProductModelDomain = await _dbContext.CartProduct.FirstOrDefaultAsync(x => x.ProductId == id && x.CartId== cart.Id);
            if (CartProductModelDomain == null) return null;
            _dbContext.CartProduct.Remove(CartProductModelDomain);
            await _dbContext.SaveChangesAsync();
            return CartProductModelDomain;
        }

        public async Task<List<Product>> GetProductsByCart(Guid id)
        {
            var cartProducts = _dbContext.CartProduct.Include("Product").AsQueryable();
            cartProducts = cartProducts.Where(x => x.CartId == id);
            var products=await cartProducts.Select(x=>x.Product).ToListAsync();
            return products;
        }

        public async Task<List<OrdersResponse>> GetCartproductsByUser(string userId,string status  )
        {

            /* var cartProducts = from c in _dbContext.Cart
                         join cp in _dbContext.CartProduct on c.Id equals cp.CartId
                         join p in _dbContext.Products on cp.ProductId equals p.Id
                         where c.Status!="active"
                         select new CartproductResponse
                         {
                          Id= p.Id,
                          Name=p.Name,
                          ImageUrl=p.ImageUrl,
                          Description= p.Description,
                          Category=p.Category,
                          Price=p.Price,
                          CartId=cp.CartId,
                          Status=c.Status,
                          Total=c.Total
                         };
             return await cartProducts.ToListAsync();*/
            bool filter = false;

            if (status == null)
            {
                filter = true;
            }
            
            var carts =await _dbContext.Cart.Where(x => x.UserId == userId).ToListAsync();
            var result = new List<OrdersResponse>();
            foreach (var cart in carts) 
            {
                var cartProducts =
                                   from cp in _dbContext.CartProduct
                                   join p in _dbContext.Products on cp.ProductId equals p.Id
                                   join s in _dbContext.Store on p.StoreId equals s.Id
                                   where cp.CartId == cart.Id &&(cart.Status==status||(filter && cart.Status != "active")) 
                                   select new CartproductResponse
                                   {
                                       Id = p.Id,
                                       Name = p.Name,
                                       ImageUrl = p.ImageUrl,
                                       Description = p.Description,
                                       Category = p.Category,
                                       Price = p.Price,
                                       storename=s.Name,
                                       quantity=1

                                       
                                  };
                if (cartProducts.Any())
                {
                    result.Add(
                            new OrdersResponse
                            {

                                products = await cartProducts.ToListAsync(),
                                CartId = cart.Id,
                                Status = cart.Status,
                                Total = cart.Total,
                                date=cart.CreatedAt
                                
                            }
                    );
                }
            }
           
            if ( result.Count == 0 )
            {
                return null;
            }
            return result.ToList();
        }

        public async Task<OrdersResponse> GetCartproductsByCartId(Guid id)
        {
            
            var cart = await _dbContext.Cart.Where(x => x.Id == id).FirstOrDefaultAsync();
            var cartProducts =
                                  from cp in _dbContext.CartProduct
                                  join p in _dbContext.Products on cp.ProductId equals p.Id
                                  join s in _dbContext.Store on p.StoreId equals s.Id
                                  where cp.CartId == cart.Id &&  cart.Status != "active"
                                  select new CartproductResponse
                                  {
                                      Id = p.Id,
                                      Name = p.Name,
                                      ImageUrl = p.ImageUrl,
                                      Description = p.Description,
                                      Category = p.Category,
                                      Price = p.Price,
                                      storename = s.Name,
                                      quantity = 1


                                  };

        var result= new OrdersResponse
            {

                products = await cartProducts.ToListAsync(),
                CartId = cart.Id,
                Status = cart.Status,
                Total = cart.Total,
                date = cart.CreatedAt

            };
               
            
            return result;
            
        }
    }
}
                    