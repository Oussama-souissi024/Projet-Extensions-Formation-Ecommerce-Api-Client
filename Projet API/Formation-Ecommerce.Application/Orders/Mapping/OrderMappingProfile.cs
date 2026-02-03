using AutoMapper;
using Formation_Ecommerce_11_2025.Application.Orders.Dtos;
using Formation_Ecommerce_11_2025.Core.Entities.Orders;

namespace Formation_Ecommerce_11_2025.Application.Orders.Mapping
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<OrderHeader, OrderHeaderDto>()
                .ReverseMap()
                .ForMember(dest => dest.OrderDetails, opt => opt.Ignore());
            CreateMap<OrderDetails, OrderDetailsDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
                .ForMember(dest => dest.ProductUrl, opt => opt.MapFrom(src => src.Product != null ? src.Product.ImageUrl : string.Empty))
                .ReverseMap()
                .ForMember(dest => dest.Product, opt => opt.Ignore());
        }
    }
}
