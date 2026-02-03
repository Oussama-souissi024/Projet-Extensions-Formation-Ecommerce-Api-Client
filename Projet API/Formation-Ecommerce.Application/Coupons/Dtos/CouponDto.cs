using System;

namespace Formation_Ecommerce_11_2025.Application.Coupons.Dtos
{
    public class CouponDto
    {
        public Guid Id { get; set; }
        public string CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal MinimumAmount { get; set; }
    }
}
