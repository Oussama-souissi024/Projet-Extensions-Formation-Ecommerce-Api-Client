namespace Formation_Ecommerce_Client.Models.ViewModels.Auth
{
    /// <summary>
    /// Modèle de présentation côté Client MVC : email saisi pour déclencher le workflow « mot de passe oublié » via l'API.
    /// </summary>
    /// <remarks>
    /// L'UI transmet l'email à l'API ; celle-ci décide ensuite de générer (ou non) un lien, sans révéler l'existence du compte.
    /// </remarks>
    public class ForgotPasswordViewModel
    {
        public string Email { get; set; } = string.Empty;
    }
}
