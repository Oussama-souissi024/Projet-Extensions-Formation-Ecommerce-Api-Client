using System;
using Formation_Ecommerce_11_2025.Application.Products.Dtos;

namespace Formation_Ecommerce_11_2025.Application.Cart.Dtos
{
    public class CartDetailsDto
    {
        public Guid Id { get; set; }
        public Guid CartHeaderId { get; set; }
        public Guid ProductId { get; set; }
        public int Count { get; set; }

        // For UI display purposes
        public ProductDto? Product { get; set; }

        // Additional calculated properties for UI
        public decimal? Price { get; set; }
    }
}
