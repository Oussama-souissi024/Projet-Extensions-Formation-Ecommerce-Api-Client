namespace Formation_Ecommerce_11_2025.API.Models
{
    // ==============================================================================================
    // 📘 CONCEPT PÉDAGOGIQUE : Standardisation des Réponses API (Wrapper Pattern)
    // ==============================================================================================
    // Problème : Si chaque endpoint renvoie des données brutes (liste, objet, string...), le client
    // (React, Angular, Mobile) doit deviner si c'est une erreur ou un succès juste avec le code HTTP.
    // De plus, les messages d'erreur ne sont pas structurés pareil.
    //
    // Solution : On enveloppe TOUTES nos réponses dans une classe générique `ApiResponse<T>`.
    // Ainsi, le client sait toujours qu'il recevra un JSON avec :
    // - Success : Vrai ou Faux
    // - Message : Info lisible
    // - Data : La donnée réelle (Produit, Commande, etc.) ou null en cas d'erreur
    // - Errors : Liste de détails d'erreurs (utile pour les formulaires)
    // ==============================================================================================
    /// <summary>
    /// Wrapper générique de réponse API : standardise le format des réponses JSON renvoyées au client.
    /// </summary>
    /// <remarks>
    /// Différences pédagogiques vs le projet monolithique MVC :
    /// - En MVC, une action retourne souvent une View/Redirect (HTML). Ici, on uniformise la réponse JSON.
    /// - Le client MVC séparé (ou tout autre client) peut traiter les succès/erreurs de manière identique quel que soit l'endpoint.
    /// - Ce modèle est propre à la couche Présentation (API) et n'a pas vocation à être persisté en base.
    /// </remarks>
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }
    }
}
