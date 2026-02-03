using Formation_Ecommerce_11_2025.Core.Entities.Cart;
using Formation_Ecommerce_11_2025.Core.Interfaces.Repositories;

namespace Formation_Ecommerce_11_2025.Test.Fakes;

/// <summary>
/// Fake (en mémoire) de <see cref="ICartRepository" />.
/// </summary>
/// <remarks>
/// Objectif pédagogique :
/// - Simuler la persistance du panier sans EF Core.
/// - Permettre les tests unitaires du CartService.
/// </remarks>
public class FakeCartRepository : ICartRepository
{
    private readonly Dictionary<Guid, CartHeader> _headers = new();
    private readonly Dictionary<Guid, CartDetails> _details = new();

    // CartHeader operations
    public Task<CartHeader?> GetCartHeaderByUserIdAsync(string userId)
    {
        var cart = _headers.Values.FirstOrDefault(c => c.UserID == userId);
        return Task.FromResult(cart);
    }

    public Task<CartHeader?> GetCartHeaderByCartHeaderIdAsync(Guid cartHeaderId)
    {
        _headers.TryGetValue(cartHeaderId, out var cart);
        return Task.FromResult(cart);
    }

    public Task<CartHeader> AddCartHeaderAsync(CartHeader cartHeader)
    {
        if (cartHeader.Id == Guid.Empty)
            cartHeader.Id = Guid.NewGuid();

        cartHeader.CartDetails ??= new List<CartDetails>();
        _headers[cartHeader.Id] = cartHeader;
        return Task.FromResult(cartHeader);
    }

    public Task<CartHeader?> UpdateCartHeaderAsync(CartHeader cartHeader)
    {
        if (_headers.ContainsKey(cartHeader.Id))
        {
            _headers[cartHeader.Id] = cartHeader;
            return Task.FromResult<CartHeader?>(cartHeader);
        }
        return Task.FromResult<CartHeader?>(null);
    }

    public Task<CartHeader?> RemoveCartHeaderAsync(CartHeader cartHeader)
    {
        if (_headers.Remove(cartHeader.Id))
        {
            // Remove associated details
            var detailsToRemove = _details.Values
                .Where(d => d.CartHeaderId == cartHeader.Id)
                .ToList();
            foreach (var detail in detailsToRemove)
            {
                _details.Remove(detail.Id);
            }
            return Task.FromResult<CartHeader?>(cartHeader);
        }
        return Task.FromResult<CartHeader?>(null);
    }

    // CartDetails operations
    public Task<IEnumerable<CartDetails>> GetListCartDetailsByCartHeaderIdAsync(Guid cartHeaderId)
    {
        var details = _details.Values
            .Where(d => d.CartHeaderId == cartHeaderId)
            .ToList();
        return Task.FromResult<IEnumerable<CartDetails>>(details);
    }

    public Task<CartDetails> AddCartDetailsAsync(CartDetails cartDetails)
    {
        if (cartDetails.Id == Guid.Empty)
            cartDetails.Id = Guid.NewGuid();

        _details[cartDetails.Id] = cartDetails;
        return Task.FromResult(cartDetails);
    }

    public Task<CartDetails?> UpdateCartDetailsAsync(CartDetails cartDetails)
    {
        if (_details.ContainsKey(cartDetails.Id))
        {
            _details[cartDetails.Id] = cartDetails;
            return Task.FromResult<CartDetails?>(cartDetails);
        }
        return Task.FromResult<CartDetails?>(null);
    }

    public Task<CartDetails?> RemoveCartDetailsAsync(CartDetails cartDetails)
    {
        if (_details.Remove(cartDetails.Id))
        {
            return Task.FromResult<CartDetails?>(cartDetails);
        }
        return Task.FromResult<CartDetails?>(null);
    }

    public Task<CartDetails?> GetCartDetailsByCartHeaderIdAndProductId(Guid cartHeaderId, Guid productId)
    {
        var detail = _details.Values.FirstOrDefault(d => 
            d.CartHeaderId == cartHeaderId && d.ProductId == productId);
        return Task.FromResult(detail);
    }

    public Task<CartDetails?> GetCartDetailsByCartDetailsId(Guid cartDetailsId)
    {
        _details.TryGetValue(cartDetailsId, out var detail);
        return Task.FromResult(detail);
    }

    public Task<bool> ClearCartAsync(string userId)
    {
        var header = _headers.Values.FirstOrDefault(h => h.UserID == userId);
        if (header != null)
        {
            var detailsToRemove = _details.Values
                .Where(d => d.CartHeaderId == header.Id)
                .ToList();
            foreach (var detail in detailsToRemove)
            {
                _details.Remove(detail.Id);
            }
            _headers.Remove(header.Id);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<int> TotalCountofCartItemAsync(Guid cartHeaderId)
    {
        var count = _details.Values.Count(d => d.CartHeaderId == cartHeaderId);
        return Task.FromResult(count);
    }
}
