
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Formation_Ecommerce_11_2025.Core.Common;

namespace Formation_Ecommerce_11_2025.Core.Entities.Coupon
{
    public class Coupon : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string CouponCode { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal DiscountAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal MinimumAmount { get; set; }

    }
}
