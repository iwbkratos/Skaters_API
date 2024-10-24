using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Skaters.Data;
using Skaters.Models.Domain;
using Skaters.Models.DTO.CartDTOs;

namespace Skaters.Repositories.CartRepositories
{
    public class SQLCartRepository : ICartRepository
    {
        private readonly SkatersAuthDbContext _dbContext;
        private readonly IMapper _mapper;

        
        public SQLCartRepository(SkatersAuthDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public async Task<CartDto> CreateAsync(AddCartRequestDto addCartRequestDto,string userID)
        {

            var cartDomainModel = _mapper.Map<Cart>(addCartRequestDto);
            cartDomainModel.UserId = userID;
            await  _dbContext.Cart.AddAsync(cartDomainModel);
            await  _dbContext.SaveChangesAsync();
            var cartDto = _mapper.Map<CartDto>(cartDomainModel);
            return cartDto;

            
        }



        public async Task<List<Cart>> GetAllAsync()
        {
            var cartModelDomain = await _dbContext.Cart.Include("Product").ToListAsync(); 

            return cartModelDomain;

        }

        public async Task<CartDto> UpdateAsync(Guid id, AddCartRequestDto updateCartRequestDto)
        {
            var ProductModelDomain = await _dbContext.Cart.FirstOrDefaultAsync(x => x.Id == id);
            if (ProductModelDomain == null)
            {
                return null;
            }
            ProductModelDomain.Status=updateCartRequestDto.Status;
            ProductModelDomain.Total=updateCartRequestDto.Total;
               await _dbContext.SaveChangesAsync();
            var cartDto=_mapper.Map<CartDto>(ProductModelDomain);
            return cartDto;
        }

        public async Task<CartDto?> DeleteAsync(Guid id)
        {
            var cartModelDomain =await _dbContext.Cart.FirstOrDefaultAsync(x => x.Id == id);
            if (cartModelDomain == null) return null;
            _dbContext.Cart.Remove(cartModelDomain);
             await _dbContext.SaveChangesAsync();
            var cartDto = _mapper.Map<CartDto>(cartModelDomain);
            return cartDto;
        }

        public async Task<CartDto> getCart(string userId)
        {
            var cartModelDomain =await _dbContext.Cart.FirstOrDefaultAsync(x=>x.Status=="active");
           
            if (cartModelDomain == null)
            {
                var newCart = new AddCartRequestDto { Status = "active", Total = 0 };
                 var createdCart= await CreateAsync(newCart, userId);
                return createdCart;
            }
            var cart= _mapper.Map<CartDto>(cartModelDomain);
            return cart;

        }
    }
}
