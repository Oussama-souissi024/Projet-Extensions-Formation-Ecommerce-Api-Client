using AutoMapper;
using Formation_Ecommerce_11_2025.Application.Coupons.Dtos;
using Formation_Ecommerce_11_2025.Core.Entities.Coupon;

namespace Formation_Ecommerce_11_2025.Application.Coupons.Mapping
{
    public class CouponProfile : Profile
    {
        public CouponProfile()
        {
            // Entity <-> DTO
            CreateMap<Coupon, CouponDto>();
            CreateMap<CouponDto, Coupon>();

            // Create/Update
            CreateMap<CreateCouponDto, Coupon>();
            CreateMap<UpdateCouponDto, Coupon>();
        }
    }
}
