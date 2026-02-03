using System.Collections.Generic;

namespace Formation_Ecommerce_Client.Models.ViewModels.Auth
{
    /// <summary>
    /// Modèle côté Client MVC : réponse de l'API lors d'une connexion, contenant le JWT et les informations utilisateur.
    /// </summary>
    /// <remarks>
    /// Différences pédagogiques vs le projet monolithique MVC :
    /// - Au lieu d'un cookie Identity géré par le serveur MVC, l'API renvoie un JWT (token) que le client stocke et renvoie en Bearer.
    /// - Les rôles (claims) permettent d'adapter l'IHM (menus/boutons), sans remplacer les contrôles d'autorisation côté API.
    /// - <see cref="ExpiresAt"/> aide le client à détecter un token expiré (et à demander une reconnexion).
    /// </remarks>
    public class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
        public DateTime ExpiresAt { get; set; }
    }
}
