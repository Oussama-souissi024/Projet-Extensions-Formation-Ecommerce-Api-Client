using System;

namespace Formation_Ecommerce_Client.Models.ViewModels.Coupons
{
    public class CouponViewModel
    {
        public Guid Id { get; set; }
        public string CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal MinimumAmount { get; set; }
    }
}
