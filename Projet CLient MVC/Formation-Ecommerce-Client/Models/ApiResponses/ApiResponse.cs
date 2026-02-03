namespace Formation_Ecommerce_Client.Models.ApiResponses
{
    /// <summary>
    /// Modèle générique côté Client MVC : représente l'enveloppe standard des réponses JSON renvoyées par l'API.
    /// </summary>
    /// <remarks>
    /// Points pédagogiques :
    /// - Le client reçoit généralement une structure { success, message, data, errors } au lieu d'un simple objet.
    /// - Cette enveloppe uniformise la gestion des erreurs (messages, liste d'erreurs) et du payload (<typeparamref name="T"/>).
    /// - Dans le monolithe, une action MVC renvoyait une vue directement ; ici on manipule d'abord des réponses HTTP/JSON.
    /// </remarks>
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T Data { get; set; } = default!;
        public List<string> Errors { get; set; } = new List<string>();
    }
}
