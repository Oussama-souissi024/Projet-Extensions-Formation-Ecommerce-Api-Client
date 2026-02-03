namespace Formation_Ecommerce_Client.Models.ViewModels.Orders
{
    /// <summary>
    /// Modèle de présentation côté Client MVC : informations saisies au checkout pour créer une commande via l'API.
    /// </summary>
    /// <remarks>
    /// Points pédagogiques :
    /// - Le client collecte les coordonnées (nom, téléphone, email) puis délègue la création de la commande à l'API.
    /// - Le panier (lignes, totaux) est généralement récupéré côté API ; ce modèle porte surtout les champs de formulaire.
    /// </remarks>
    public class CreateOrderViewModel
    {
        public Guid UserId { get; set; }
        public string? PromoCode { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
