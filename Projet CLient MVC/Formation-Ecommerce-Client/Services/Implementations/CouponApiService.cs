using Formation_Ecommerce_Client.Models.ApiResponses;
using Formation_Ecommerce_Client.Models.ViewModels.Coupons;
using System.Net.Http.Headers;

namespace Formation_Ecommerce_Client.Services.Implementations
{
    public interface ICouponApiService
    {
        Task<IEnumerable<CouponViewModel>> GetAllAsync();
        Task<CouponViewModel?> GetByIdAsync(Guid id);
        Task<CouponViewModel?> GetByCodeAsync(string code);
        Task CreateAsync(CreateCouponViewModel model);
        Task UpdateAsync(Guid id, UpdateCouponViewModel model);
        Task DeleteAsync(Guid id);
    }

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

