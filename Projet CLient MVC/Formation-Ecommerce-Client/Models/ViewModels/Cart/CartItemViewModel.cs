namespace Formation_Ecommerce_Client.Models.ViewModels.Cart
{
    /// <summary>
    /// Modèle de présentation côté Client MVC : représente une ligne de panier telle qu'affichée dans l'IHM.
    /// </summary>
    /// <remarks>
    /// Différences pédagogiques vs le projet monolithique MVC :
    /// - La quantité, le prix et l'image proviennent d'une réponse API, pas d'un accès direct à la base.
    /// - Les propriétés calculées (ex: <see cref="Total"/>) sont purement UI et ne remplacent pas les calculs côté serveur.
    /// </remarks>
    public class CartItemViewModel
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Count { get; set; }
        public decimal Total => Price * Quantity;
        public string? ImageUrl { get; set; }
    }
}
