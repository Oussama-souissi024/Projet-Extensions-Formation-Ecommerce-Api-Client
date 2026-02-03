using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Formation_Ecommerce_11_2025.Application.Coupons.Dtos;

namespace Formation_Ecommerce_11_2025.Application.Coupons.Interfaces
{
    public interface ICouponService
    {
        Task<CouponDto> AddAsync(CouponDto coupon);
        Task<CouponDto?> ReadByIdAsync(Guid couponId);
        Task<CouponDto?> GetCouponByCodeAsync(string couponCode);
        Task<IEnumerable<CouponDto>> ReadAllAsync();
        Task UpdateAsync(UpdateCouponDto updateCouponDto);
        Task DeleteAsync(Guid id);
    }
}
