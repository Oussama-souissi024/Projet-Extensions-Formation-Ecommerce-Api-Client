namespace Formation_Ecommerce_Client.Models.ViewModels.Orders
{
    /// <summary>
    /// Modèle de présentation côté Client MVC : représente une ligne de commande affichée dans l'IHM.
    /// </summary>
    /// <remarks>
    /// - Les lignes proviennent des DTO renvoyés par l'API et sont converties en modèle UI.
    /// - Les calculs simples (ex: <see cref="Total"/>) sont faits pour l'affichage, tandis que la facturation reste côté API.
    /// </remarks>
    public class OrderItemViewModel
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal Total => Price * Count;
        public string? ImageUrl { get; set; }
    }
}
