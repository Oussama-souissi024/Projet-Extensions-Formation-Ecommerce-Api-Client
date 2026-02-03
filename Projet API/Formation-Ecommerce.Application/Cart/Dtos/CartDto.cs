using System.Collections.Generic;

namespace Formation_Ecommerce_11_2025.Application.Cart.Dtos
{
    public class CartDto
    {
        public CartHeaderDto CartHeader { get; set; }
        public IEnumerable<CartDetailsDto> CartDetails { get; set; }
    }
}
