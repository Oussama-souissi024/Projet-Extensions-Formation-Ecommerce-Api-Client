using System;
using System.Collections.Generic;

namespace Formation_Ecommerce_11_2025.Application.Orders.Dtos
{
    public class OrderHeaderDto
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public decimal Discount { get; set; }
        public decimal? OrderTotal { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public DateTime OrderTime { get; set; }
        public string? Status { get; set; }
        public IEnumerable<OrderDetailsDto> OrderDetails { get; set; }
    }
}
