using Formation_Ecommerce_Client.Models.ApiResponses;
using Formation_Ecommerce_Client.Models.ViewModels.Coupons;
using System.Net.Http.Headers;

namespace Formation_Ecommerce_Client.Services.Implementations
{
    /// <summary>
    /// Contrat du service HTTP coupons côté Client MVC : expose les opérations de gestion et de validation de coupons via l'API.
    /// </summary>
    /// <remarks>
    /// Points pédagogiques :
    /// - Les contrôleurs MVC appellent ce service au lieu d'accéder à une base de données.
    /// - Le service gère la communication HTTP (routes, JSON) et l'authentification Bearer (JWT) pour les actions protégées.
    /// </remarks>
    public interface ICouponApiService
    {
        Task<IEnumerable<CouponViewModel>> GetAllAsync();
        Task<CouponViewModel?> GetByIdAsync(Guid id);
        Task<CouponViewModel?> GetByCodeAsync(string code);
        Task CreateAsync(CreateCouponViewModel model);
        Task UpdateAsync(Guid id, UpdateCouponViewModel model);
        Task DeleteAsync(Guid id);
    }

    /// <summary>
    /// Implémentation du service HTTP coupons : consomme les endpoints REST et renvoie des ViewModels utilisables par l'IHM.
    /// </summary>
    /// <remarks>
    /// Différences pédagogiques vs le projet monolithique MVC :
    /// - La logique métier (ex: coupon inexistant) est gérée côté API ; ici on traduit surtout des statuts HTTP en résultats UI.
    /// - Le token JWT est envoyé en en-tête pour les actions d'administration (création/édition/suppression).
    /// </remarks>
    public class CouponApiService : ICouponApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CouponApiService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _httpContextAccessor = httpContextAccessor;
             var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<IEnumerable<CouponViewModel>> GetAllAsync()
        {
             var response = await _httpClient.GetAsync("coupons");
             if(response.StatusCode == System.Net.HttpStatusCode.NotFound) return new List<CouponViewModel>();
             response.EnsureSuccessStatusCode();
             var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<CouponViewModel>>>();
             return result?.Data ?? Array.Empty<CouponViewModel>();
        }

        public async Task<CouponViewModel?> GetByIdAsync(Guid id)
        {
             var response = await _httpClient.GetAsync($"coupons/{id}");
             if(response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
             response.EnsureSuccessStatusCode();
             var result = await response.Content.ReadFromJsonAsync<ApiResponse<CouponViewModel>>();
             return result?.Data;
        }

        public async Task<CouponViewModel?> GetByCodeAsync(string code)
        {
             var response = await _httpClient.GetAsync($"coupons/validate/{code}");
             if(response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
             response.EnsureSuccessStatusCode();
             var result = await response.Content.ReadFromJsonAsync<ApiResponse<CouponViewModel>>();
             return result?.Data;
        }

        public async Task CreateAsync(CreateCouponViewModel model)
        {
            var payload = new
            {
                CouponCode = model.CouponCode,
                DiscountAmount = model.DiscountAmount,
                MinimumAmount = model.MinimumAmount
            };

            var response = await _httpClient.PostAsJsonAsync("coupons", payload);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(Guid id, UpdateCouponViewModel model)
        {
            var payload = new
            {
                CouponCode = model.CouponCode,
                DiscountAmount = model.DiscountAmount,
                MinimumAmount = model.MinimumAmount
            };

            var response = await _httpClient.PutAsJsonAsync($"coupons/{id}", payload);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(Guid id)
        {
             var response = await _httpClient.DeleteAsync($"coupons/{id}");
             response.EnsureSuccessStatusCode();
        }
    }
}

