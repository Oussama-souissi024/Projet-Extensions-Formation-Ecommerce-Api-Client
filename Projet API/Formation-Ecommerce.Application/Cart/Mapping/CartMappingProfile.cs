using AutoMapper;
using Formation_Ecommerce_11_2025.Application.Cart.Dtos;
using Formation_Ecommerce_11_2025.Core.Entities.Cart;

namespace Formation_Ecommerce_11_2025.Application.Cart.Mapping
{
    public class CartMappingProfile : Profile
    {
        public CartMappingProfile()
        {
            // Map CartHeader <-> CartHeaderDto
            CreateMap<CartHeader, CartHeaderDto>().ReverseMap();

            // Map CartDetails <-> CartDetailsDto
            CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
        }
    }
}
