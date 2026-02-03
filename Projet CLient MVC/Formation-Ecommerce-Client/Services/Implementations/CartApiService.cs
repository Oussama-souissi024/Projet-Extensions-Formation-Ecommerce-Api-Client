using Formation_Ecommerce_Client.Models.ApiResponses;
using Formation_Ecommerce_Client.Models.ApiDtos.Cart;
using Formation_Ecommerce_Client.Models.ViewModels.Cart;
using System.Net.Http.Headers;

namespace Formation_Ecommerce_Client.Services.Implementations
{
    /// <summary>
    /// Contrat du service HTTP panier côté Client MVC : regroupe les opérations de lecture/mutation du panier via l'API.
    /// </summary>
    /// <remarks>
    /// Points pédagogiques :
    /// - Le panier n'est pas stocké dans l'application MVC : toutes les actions passent par des endpoints REST sécurisés.
    /// - Le service masque les détails HttpClient (routes, JSON) et renvoie des ViewModels prêts pour l'IHM.
    /// - Le JWT est envoyé en Bearer (si présent en session) pour authentifier l'utilisateur côté API.
    /// </remarks>
    public interface ICartApiService
    {
        Task<CartViewModel> GetCartAsync();
        Task<CartViewModel> AddToCartAsync(AddToCartViewModel model);
        Task<CartViewModel> UpdateQuantityAsync(Guid itemId, int quantity);
        Task RemoveFromCartAsync(Guid itemId);
        Task<bool> ApplyCouponAsync(string couponCode);
        Task<bool> RemoveCouponAsync();
        Task ClearCartAsync();
    }

    /// <summary>
    /// Implémentation du service HTTP panier : exécute les appels vers l'API et mappe les DTO API vers des ViewModels UI.
    /// </summary>
    /// <remarks>
    /// Différences pédagogiques vs le projet monolithique MVC :
    /// - Ici, pas d'accès direct à EF Core : toute la persistance est déportée côté API.
    /// - L'authentification se fait par JWT : le service configure l'en-tête <c>Authorization: Bearer</c> au démarrage.
    /// - Le mapping (DTO -&gt; ViewModel) permet de découpler l'IHM des contrats API et d'adapter l'affichage sans toucher au serveur.
    /// </remarks>
    public class CartApiService : ICartApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartApiService(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _httpContextAccessor = httpContextAccessor;

            var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<CartViewModel> GetCartAsync()
        {
            var response = await _httpClient.GetAsync("cart");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return new CartViewModel();

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<CartDto>>();
            return MapToViewModel(result?.Data);
        }

        public async Task<CartViewModel> AddToCartAsync(AddToCartViewModel model)
        {
            var payload = new CartDto
            {
                CartHeader = new CartHeaderDto(),
                CartDetails = new[]
                {
                    new CartDetailsDto
                    {
                        ProductId = model.ProductId,
                        Count = model.Quantity
                    }
                }
            };

            var response = await _httpClient.PostAsJsonAsync("cart", payload);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<CartDto>>();
            return MapToViewModel(result?.Data);
        }

        public async Task<CartViewModel> UpdateQuantityAsync(Guid itemId, int quantity)
        {
            if (quantity < 1)
            {
                await RemoveFromCartAsync(itemId);
                return await GetCartAsync();
            }

            var existing = await GetCartDtoAsync();
            if (existing == null)
                return new CartViewModel();

            var details = existing.CartDetails?.ToList() ?? new List<CartDetailsDto>();
            var line = details.FirstOrDefault(d => d.Id == itemId);
            if (line == null)
                return MapToViewModel(existing);

            line.Count = quantity;

            var payload = new CartDto
            {
                CartHeader = existing.CartHeader ?? new CartHeaderDto(),
                CartDetails = details
            };

            var response = await _httpClient.PostAsJsonAsync("cart", payload);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<CartDto>>();
            return MapToViewModel(result?.Data);
        }

        public async Task RemoveFromCartAsync(Guid itemId)
        {
            var response = await _httpClient.DeleteAsync($"cart/items/{itemId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> ApplyCouponAsync(string couponCode)
        {
            var payload = new ApplyCouponRequest { CouponCode = couponCode };
            var response = await _httpClient.PostAsJsonAsync("cart/apply-coupon", payload);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveCouponAsync()
        {
            var response = await _httpClient.PostAsync("cart/remove-coupon", null);
            return response.IsSuccessStatusCode;
        }

        public async Task ClearCartAsync()
        {
            var response = await _httpClient.DeleteAsync("cart");
            response.EnsureSuccessStatusCode();
        }

        private async Task<CartDto?> GetCartDtoAsync()
        {
            var response = await _httpClient.GetAsync("cart");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<CartDto>>();
            return result?.Data;
        }

        private static CartViewModel MapToViewModel(CartDto? dto)
        {
            if (dto == null)
                return new CartViewModel();

            var header = dto.CartHeader;

            var items = (dto.CartDetails ?? Enumerable.Empty<CartDetailsDto>())
                .Select(d =>
                {
                    var product = d.Product;
                    var price = d.Price ?? product?.Price ?? 0m;
                    return new CartItemViewModel
                    {
                        Id = d.Id,
                        ProductId = d.ProductId,
                        ProductName = product?.Name ?? string.Empty,
                        Description = product?.Description ?? string.Empty,
                        Price = price,
                        Count = d.Count,
                        ImageUrl = product?.ImageUrl ?? string.Empty
                    };
                })
                .ToList();

            var total = header?.CartTotal ?? items.Sum(i => i.Price * i.Count);
            var discount = header?.Discount ?? 0m;

            var vm = new CartViewModel
            {
                TotalAmount = total,
                Discount = discount,
                GrandTotal = total - discount,
                CouponCode = header?.CouponCode,
                Items = items
            };

            if (header != null && !string.IsNullOrWhiteSpace(header.UserID))
                vm.UserId = header.UserID;

            return vm;
        }
    }
}
