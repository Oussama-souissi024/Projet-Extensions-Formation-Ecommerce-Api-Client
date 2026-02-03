using System.Collections.Generic;

namespace Formation_Ecommerce_Client.Models.ViewModels.Auth
{
    public class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
        public DateTime ExpiresAt { get; set; }
    }
}
