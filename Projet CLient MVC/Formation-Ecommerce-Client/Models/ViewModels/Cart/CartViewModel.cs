using Formation_Ecommerce_Client.Models.ApiDtos.Cart;

namespace Formation_Ecommerce_Client.Models.ViewModels.Cart
{
    /// <summary>
    /// Modèle de présentation côté Client MVC : version « UI-friendly » du panier (totaux, items, coupon).
    /// </summary>
    /// <remarks>
    /// Points pédagogiques :
    /// - L'API reste la source de vérité (prix, remises, validité coupon) ; ce modèle sert à l'affichage et à la saisie.
    /// - La présence de <see cref="CartDetailsDto"/> reflète que certaines vues peuvent réutiliser des structures API.
    /// </remarks>
    public class CartViewModel
    {
        public Guid CartHeaderId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public decimal OrderTotal { get; set; }
        public string? CouponCode { get; set; }
        public decimal Discount { get; set; }
        public List<CartDetailsDto> CartDetails { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public List<CartItemViewModel> Items { get; set; } = new();
    }

    /// <summary>
    /// Modèle de présentation côté Client MVC : payload minimal pour demander à l'API l'ajout d'un produit au panier.
    /// </summary>
    /// <remarks>
    /// Ce type est typiquement construit depuis un bouton « Ajouter au panier » et envoyé au service HTTP panier.
    /// </remarks>
    public class AddToCartViewModel
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
