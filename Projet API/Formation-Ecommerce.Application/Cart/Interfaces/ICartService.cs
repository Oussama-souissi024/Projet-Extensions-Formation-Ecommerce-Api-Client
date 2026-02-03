using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Formation_Ecommerce_11_2025.Application.Cart.Dtos;

namespace Formation_Ecommerce_11_2025.Application.Cart.Interfaces
{
    public interface ICartService
    {
        Task<CartDto?> GetCartByUserIdAsync(string userId);
        Task<CartDto> UpsertCartAsync(CartDto cartDto);
        Task<CartDto?> ApplyCouponAsync(string userId, string couponCode);
        Task<bool> RemoveCartItemAsync(Guid cartDetailsId);
        Task<bool> ClearCartAsync(string userId);
    }
}
