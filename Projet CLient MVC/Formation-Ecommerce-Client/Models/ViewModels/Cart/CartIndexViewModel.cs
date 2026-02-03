using System;
using System.Collections.Generic;
using Formation_Ecommerce_Client.Models.ApiDtos.Cart;

namespace Formation_Ecommerce_Client.Models.ViewModels.Cart
{
    public class CartIndexViewModel
    {
        public CartHeaderDto? CartHeader { get; set; }
        public List<CartDetailsDto> CartDetails { get; set;} = new();
    }
}

