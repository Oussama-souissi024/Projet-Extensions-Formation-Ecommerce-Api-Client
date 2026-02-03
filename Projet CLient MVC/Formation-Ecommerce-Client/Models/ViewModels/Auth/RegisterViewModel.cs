namespace Formation_Ecommerce_11_2025.Models.Auth
{
    /// <summary>
    /// Modèle de présentation côté Client MVC : données du formulaire d'inscription envoyées à l'API.
    /// </summary>
    /// <remarks>
    /// Points pédagogiques :
    /// - Le client collecte les informations (email, téléphone, mot de passe, rôle) puis délègue la création du compte à l'API.
    /// - Les validations (unicité email, robustesse mot de passe, confirmation email) sont appliquées côté serveur.
    /// </remarks>
    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string? Role { get; set; }
    }
}
