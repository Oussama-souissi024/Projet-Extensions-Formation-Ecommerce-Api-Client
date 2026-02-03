namespace Formation_Ecommerce_Client.Models.ApiDtos.Orders
{
    /// <summary>
    /// DTO côté Client MVC : en-tête de commande échangé avec l'API (données client + statut + totaux).
    /// </summary>
    /// <remarks>
    /// Points pédagogiques :
    /// - Le Client MVC ne persiste pas la commande : il envoie/reçoit ce DTO via HTTP (JSON).
    /// - Le statut et les totaux sont la source de vérité côté API ; le client les affiche et déclenche des actions (annuler, valider...).
    /// </remarks>
    public class OrderHeaderDto
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public string? CouponCode { get; set; }
        public decimal Discount { get; set; }
        public decimal? OrderTotal { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public DateTime OrderTime { get; set; }
        public string? Status { get; set; }
        public IEnumerable<OrderDetailsDto> OrderDetails { get; set; } = new List<OrderDetailsDto>();
    }

    /// <summary>
    /// DTO côté Client MVC : détail d'une ligne de commande (produit, quantité, prix) renvoyé par l'API.
    /// </summary>
    /// <remarks>
    /// Dans une UI, ce type sert principalement à afficher le récapitulatif de commande et à calculer des sous-totaux côté écran.
    /// </remarks>
    public class OrderDetailsDto
    {
        public Guid Id { get; set; }
        public Guid OrderHeaderId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Count { get; set; }
    }

    /// <summary>
    /// DTO côté Client MVC : payload simplifié pour demander à l'API une mise à jour de statut (workflow admin).
    /// </summary>
    /// <remarks>
    /// Le contrôle des règles (transitions autorisées, rôles) reste côté API ; le client ne fait qu'émettre la demande.
    /// </remarks>
    public class UpdateStatusRequest
    {
        public string Status { get; set; } = "";
    }
}
