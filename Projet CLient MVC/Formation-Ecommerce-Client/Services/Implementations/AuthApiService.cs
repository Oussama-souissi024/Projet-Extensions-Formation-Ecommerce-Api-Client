using Formation_Ecommerce_11_2025.Models.Auth;
using Formation_Ecommerce_Client.Models.ApiResponses;
using Formation_Ecommerce_Client.Models.ViewModels.Auth;

namespace Formation_Ecommerce_Client.Services.Implementations
{
    public interface IAuthApiService
    {
        Task<TokenResponse> LoginAsync(LoginViewModel model);
        Task<bool> RegisterAsync(RegisterViewModel model);
        Task LogoutAsync();
        Task<bool> ConfirmEmailAsync(string userId, string token);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(ResetPasswordViewModel model);
    }

    public class AuthApiService : IAuthApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthApiService(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TokenResponse> LoginAsync(LoginViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/login", model);
            
            if (!response.IsSuccessStatusCode)
            {
                var message = response.StatusCode >= System.Net.HttpStatusCode.InternalServerError
                    ? "Erreur serveur API. Vérifiez que l'API et SQL Server sont démarrés (migrations/connexion)."
                    : "Identifiants invalides";

                try
                {
                    var error = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                    if (!string.IsNullOrWhiteSpace(error?.Message))
                    {
                        message = error.Message;
                    }
                    else if (error?.Errors != null && error.Errors.Count > 0)
                    {
                        message = string.Join(" | ", error.Errors);
                    }
                }
                catch
                {
                    // Ignore parsing errors and keep default message
                }

                message = $"{message} (HTTP {(int)response.StatusCode})";

                throw new UnauthorizedAccessException(message);
            }
            
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<TokenResponse>>();
            
            // Stocker le token en session
            if (result?.Data != null)
            {
                _httpContextAccessor.HttpContext?.Session.SetString("JwtToken", result.Data.Token);
                // On pourrait aussi stocker le RefreshToken
            }

            return result?.Data ?? throw new InvalidOperationException("Réponse API invalide lors du login");
        }

        public async Task<bool> RegisterAsync(RegisterViewModel model)
        {
            var payload = new
            {
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Password = model.Password,
                Role = model.Role
            };

            var response = await _httpClient.PostAsJsonAsync("auth/register", payload);

            if (!response.IsSuccessStatusCode)
            {
                var message = response.StatusCode >= System.Net.HttpStatusCode.InternalServerError
                    ? "Erreur serveur API. Vérifiez que l'API et SQL Server sont démarrés (migrations/connexion)."
                    : "Échec de l'inscription";

                try
                {
                    var error = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                    if (!string.IsNullOrWhiteSpace(error?.Message))
                    {
                        message = error.Message;
                    }
                    else if (error?.Errors != null && error.Errors.Count > 0)
                    {
                        message = string.Join(" | ", error.Errors);
                    }
                }
                catch
                {
                }

                message = $"{message} (HTTP {(int)response.StatusCode})";
                throw new InvalidOperationException(message);
            }

            return true;
        }

        public Task LogoutAsync()
        {
            _httpContextAccessor.HttpContext?.Session.Remove("JwtToken");
            return Task.CompletedTask;
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            var response = await _httpClient.GetAsync($"auth/confirm-email?userId={userId}&token={Uri.EscapeDataString(token)}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var payload = new { Email = email };
            var response = await _httpClient.PostAsJsonAsync("auth/forgot-password", payload);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/reset-password", model);
            return response.IsSuccessStatusCode;
        }
    }
}
