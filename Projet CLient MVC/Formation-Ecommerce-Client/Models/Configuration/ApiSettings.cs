namespace Formation_Ecommerce_Client.Models.Configuration
{
    /// <summary>
    /// Modèle de configuration côté Client MVC : paramètres de connexion à l'API (base URL, timeout).
    /// </summary>
    /// <remarks>
    /// Points pédagogiques :
    /// - Dans l'architecture client/serveur, l'UI doit connaître l'URL du serveur API (souvent via appsettings).
    /// - Le timeout protège l'IHM d'appels réseau trop longs et permet de gérer une API indisponible (UX).
    /// </remarks>
    public class ApiSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public int Timeout { get; set; }
    }
}
