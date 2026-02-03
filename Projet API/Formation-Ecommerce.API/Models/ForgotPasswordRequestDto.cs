namespace Formation_Ecommerce_11_2025.API.Models
{
    /// <summary>
    /// Modèle de requête API : demande de réinitialisation de mot de passe (email uniquement).
    /// </summary>
    /// <remarks>
    /// Points pédagogiques :
    /// - Ce modèle est spécifique à l'API (Présentation) et correspond au payload JSON envoyé par le client.
    /// - La logique réelle (génération du token, envoi d'email) est gérée par la couche Application/Infrastructure.
    /// - L'API renvoie volontairement un succès même si l'email n'existe pas (bonne pratique sécurité).
    /// </remarks>
    public class ForgotPasswordRequestDto
    {
        public string Email { get; set; } = string.Empty;

    }
}
