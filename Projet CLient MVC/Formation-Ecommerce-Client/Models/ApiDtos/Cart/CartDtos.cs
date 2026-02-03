using Formation_Ecommerce_Client.Models.ViewModels.Products;

namespace Formation_Ecommerce_Client.Models.ApiDtos.Cart
{
    /// <summary>
    /// DTO côté Client MVC : représente le panier tel qu'il transite entre le Client et l'API (payload JSON).
    /// </summary>
    /// <remarks>
    /// Points pédagogiques :
    /// - Dans l'architecture client/serveur, l'UI manipule des contrats d'échange (DTO) et non des entités EF/Core.
    /// - Le DTO reflète la structure attendue par les endpoints panier (ex: <c>GET/POST /api/cart</c>).
    /// - Le mapping vers l'IHM est ensuite fait dans un ViewModel dédié (ex: <c>CartViewModel</c>).
    /// </remarks>
    public class CartDto
    {
        public CartHeaderDto CartHeader { get; set; } = new CartHeaderDto();
        public IEnumerable<CartDetailsDto> CartDetails { get; set; } = new List<CartDetailsDto>();
    }

    /// <summary>
    /// DTO côté Client MVC : en-tête de panier (identité utilisateur, coupon, totaux) renvoyé par l'API.
    /// </summary>
    /// <remarks>
    /// Ce type sert à sérialiser/désérialiser les informations globales du panier, calculées et persistées côté API.
    /// </remarks>
    public class CartHeaderDto
    {
        public Guid Id { get; set; }
        public string UserID { get; set; } = string.Empty;
        public string? CouponCode { get; set; }

        public decimal? CartTotal { get; set; }
        public decimal? Discount { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }

    /// <summary>
    /// DTO côté Client MVC : détail d'une ligne de panier (produit, quantité, prix) échangé avec l'API.
    /// </summary>
    /// <remarks>
    /// Points pédagogiques :
    /// - Les calculs de prix/remises sont faits côté serveur ; le client reçoit un instantané pour l'affichage.
    /// - La présence de <see cref="ProductViewModel"/> dans le DTO illustre qu'une API peut enrichir ses réponses.
    /// </remarks>
    public class CartDetailsDto
    {
        public Guid Id { get; set; }
        public Guid CartHeaderId { get; set; }
        public Guid ProductId { get; set; }
        public int Count { get; set; }

        public ProductViewModel? Product { get; set; }
        public decimal? Price { get; set; }
    }

    /// <summary>
    /// DTO côté Client MVC : requête minimaliste envoyée à l'API pour appliquer un code coupon au panier.
    /// </summary>
    /// <remarks>
    /// Dans l'architecture client/serveur, ce type reflète le contrat de l'endpoint (payload JSON), sans logique métier.
    /// </remarks>
    public class ApplyCouponRequest
    {
        public string CouponCode { get; set; } = "";
    }
}
