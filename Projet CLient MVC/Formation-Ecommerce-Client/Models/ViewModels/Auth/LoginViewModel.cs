namespace Formation_Ecommerce_11_2025.Models.Auth
{
    /// <summary>
    /// Modèle de présentation côté Client MVC : données du formulaire de connexion envoyées à l'API.
    /// </summary>
    /// <remarks>
    /// Différences pédagogiques vs le projet monolithique MVC :
    /// - Le formulaire ne déclenche pas un cookie d'authentification local ; les identifiants sont transmis à l'API.
    /// - L'API renvoie un JWT qui sera stocké côté client (Session) et envoyé ensuite en Bearer.
    /// </remarks>
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
