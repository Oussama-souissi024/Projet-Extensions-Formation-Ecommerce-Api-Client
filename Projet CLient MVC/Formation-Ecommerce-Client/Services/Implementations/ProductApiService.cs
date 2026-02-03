using Formation_Ecommerce_Client.Models.ApiResponses;
using Formation_Ecommerce_Client.Models.ViewModels.Products;
using Formation_Ecommerce_Client.Services.Interfaces;
using System.Net.Http.Headers;

namespace Formation_Ecommerce_Client.Services.Implementations
{
    public interface IProductApiService : IApiServiceBase<ProductViewModel, CreateProductViewModel, UpdateProductViewModel>
    {
    }

    public class ProductApiService : IProductApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductApiService(
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

        public async Task<IEnumerable<ProductViewModel>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync("products");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<ProductViewModel>>>();
            return result?.Data ?? Array.Empty<ProductViewModel>();
        }

        public async Task<ProductViewModel> GetByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"products/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductViewModel>>();
            return result?.Data ?? throw new InvalidOperationException("Réponse API invalide lors du chargement du produit");
        }

        public async Task<ProductViewModel> CreateAsync(CreateProductViewModel dto)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(dto.Name), "Name");
            content.Add(new StringContent(dto.Price.ToString()), "Price");
            content.Add(new StringContent(dto.CategoryId.ToString()), "CategoryId");
            content.Add(new StringContent(dto.CategoryId.ToString()), "CategoryID");
            if (dto.Description != null)
                content.Add(new StringContent(dto.Description), "Description");

            if (dto.ImageFile != null)
            {
                var streamContent = new StreamContent(dto.ImageFile.OpenReadStream());
                content.Add(streamContent, "ImageFile", dto.ImageFile.FileName);
            }

            var response = await _httpClient.PostAsync("products", content);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<ProductViewModel>>();
            return result?.Data ?? throw new InvalidOperationException("Réponse API invalide lors de la création du produit");
        }

        public async Task UpdateAsync(Guid id, UpdateProductViewModel dto)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(dto.Id.ToString()), "Id");
            content.Add(new StringContent(dto.Name), "Name");
            content.Add(new StringContent(dto.Price.ToString()), "Price");
            content.Add(new StringContent(dto.CategoryId.ToString()), "CategoryId");
            content.Add(new StringContent(dto.CategoryId.ToString()), "CategoryID");
             if (dto.Description != null)
                content.Add(new StringContent(dto.Description), "Description");

            if (dto.ImageFile != null)
            {
                var streamContent = new StreamContent(dto.ImageFile.OpenReadStream());
                content.Add(streamContent, "ImageFile", dto.ImageFile.FileName);
            }

            // Note: Use PutAsync or another appropriate method depending on API
            // For Multipart/form-data, typically POST or PUT is used. 
            // If the API expects JSON for update, we would use PutAsJsonAsync but here we handle file upload potential.
            // Assuming the API endpoint for update supports multipart/form-data.
            var response = await _httpClient.PutAsync($"products/{id}", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"products/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
