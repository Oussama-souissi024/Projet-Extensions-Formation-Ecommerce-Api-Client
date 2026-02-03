namespace Formation_Ecommerce_11_2025.API.Models
{
    /// <summary>
    /// Modèle de réponse de login API : contient le JWT, les informations d'identité et la date d'expiration.
    /// </summary>
    /// <remarks>
    /// Différences pédagogiques vs le projet monolithique MVC :
    /// - En MVC, l'authentification est souvent basée sur des cookies gérés par le serveur.
    /// - En API, on renvoie un token (JWT) que le client stocke et renvoie dans l'en-tête Authorization.
    /// - Le client MVC de l'extension stocke ce token (ex: cookie "non-auth") et l'ajoute dans HttpClient.
    /// </remarks>
    public class JwtLoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
        public DateTime ExpiresAt { get; set; }
    }
}
