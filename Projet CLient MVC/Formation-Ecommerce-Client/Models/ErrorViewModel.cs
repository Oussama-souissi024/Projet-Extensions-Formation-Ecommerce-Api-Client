namespace Formation_Ecommerce_Client.Models;

/// <summary>
/// Modèle de présentation pour la page d'erreur du Client MVC.
/// </summary>
/// <remarks>
/// Points pédagogiques :
/// - Ce modèle est purement UI : il sert à afficher un identifiant de requête (diagnostic) dans les vues Razor.
/// - Dans l'architecture client/serveur, les erreurs peuvent provenir du client (UI) ou de l'API ; l'IHM choisit comment les présenter.
/// </remarks>
public class ErrorViewModel
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
