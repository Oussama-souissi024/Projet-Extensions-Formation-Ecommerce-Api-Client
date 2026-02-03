using System;

namespace Formation_Ecommerce_11_2025.Application.Orders.Dtos
{
    public class OrderDetailsDto
    {
        public Guid Id { get; set; }
        public Guid OrderHeaderId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductUrl { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
    }
}
