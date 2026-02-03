using System;
using System.ComponentModel.DataAnnotations;

namespace Formation_Ecommerce_11_2025.Application.Coupons.Dtos
{
    public class CreateCouponDto
    {
        [Required]
        [MaxLength(50)]
        public string CouponCode { get; set; }

        [Required]
        public decimal DiscountAmount { get; set; }

        [Required]
        public decimal MinimumAmount { get; set; }
    }
}
