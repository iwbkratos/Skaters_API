using AutoMapper;
using Skaters.Domain.Model;
using Skaters.Models.Domain;
using Skaters.Models.DTO.CartDTOs;
using Skaters.Models.DTO.CartProductDto;
using Skaters.Models.DTO.ProductDTOs;
using Skaters.Models.DTO.StoreDTOs;

namespace Skaters.Mapping
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            //Store
            CreateMap<Store, StoreDto>().ReverseMap();
            CreateMap<Store,AddStoreRequestDto>().ReverseMap();
            CreateMap<Store, UpdateStoreRequestDto>().ReverseMap();

            //Product
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<AddorProductRequestDto, Product>().ReverseMap();

            //Cart
            CreateMap<Cart, CartDto>().ReverseMap();
            CreateMap<AddCartRequestDto, Cart>().ReverseMap();
            CreateMap<UpdateCartRequestDto, Cart>().ReverseMap();

            //CartProduct
            CreateMap<CartProduct, CartProductDto>().ReverseMap();
            CreateMap<CartProduct, CartProductDto>().ReverseMap();
            CreateMap<AddorUpdateCartProductRequestDto, CartProduct>().ReverseMap();
            


        }
    }
}
