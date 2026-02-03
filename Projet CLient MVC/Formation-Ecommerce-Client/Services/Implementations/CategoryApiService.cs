using Formation_Ecommerce_Client.Models.ApiResponses;
using Formation_Ecommerce_Client.Models.ViewModels.Categories;
using System.Net.Http.Headers;

namespace Formation_Ecommerce_Client.Services.Implementations
{
    public interface ICategoryApiService
    {
        Task<IEnumerable<CategoryViewModel>> GetAllAsync();
        Task<CategoryViewModel> GetByIdAsync(Guid id);
        Task CreateAsync(CreateCategoryViewModel model);
        Task UpdateAsync(UpdateCategoryViewModel model);
        Task DeleteAsync(Guid id);
    }

    public class CategoryApiService : ICategoryApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryApiService(
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

        public async Task<IEnumerable<CategoryViewModel>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("categories");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<CategoryViewModel>>>();
            return result?.Data ?? Array.Empty<CategoryViewModel>();
        }

        public async Task<CategoryViewModel> GetByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"categories/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<CategoryViewModel>>();
            return result?.Data ?? throw new InvalidOperationException("Réponse API invalide lors du chargement de la catégorie");
        }

        public async Task CreateAsync(CreateCategoryViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("categories", model);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(UpdateCategoryViewModel model)
        {
            var response = await _httpClient.PutAsJsonAsync($"categories/{model.Id}", model);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"categories/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
