using Formation_Ecommerce_11_2025.Core.Entities.Cart;

namespace Formation_Ecommerce_11_2025.Core.Interfaces.Repositories
{
    public interface ICartRepository
    {
        Task<CartHeader?> GetCartHeaderByUserIdAsync(string userId);
        Task<CartHeader?> GetCartHeaderByCartHeaderIdAsync(Guid cartHeaderId);

        Task<CartHeader> AddCartHeaderAsync(CartHeader cartHeader);
        Task<CartHeader?> UpdateCartHeaderAsync(CartHeader cartHeader);
        Task<CartHeader?> RemoveCartHeaderAsync(CartHeader cartHeader);

        Task<IEnumerable<CartDetails>> GetListCartDetailsByCartHeaderIdAsync(Guid cartHeaderId);

        Task<CartDetails> AddCartDetailsAsync(CartDetails cartDetails);
        Task<CartDetails?> UpdateCartDetailsAsync(CartDetails cartDetails);
        Task<CartDetails?> RemoveCartDetailsAsync(CartDetails cartDetails);

        Task<CartDetails?> GetCartDetailsByCartHeaderIdAndProductId(Guid cartHeaderId, Guid productId);
        Task<CartDetails?> GetCartDetailsByCartDetailsId(Guid cartDetailsId);

        Task<bool> ClearCartAsync(string userId);
        Task<int> TotalCountofCartItemAsync(Guid cartHeaderId);
    }
}
