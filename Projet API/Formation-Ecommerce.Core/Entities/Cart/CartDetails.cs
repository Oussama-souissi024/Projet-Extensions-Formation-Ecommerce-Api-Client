using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Formation_Ecommerce_11_2025.Core.Common;

namespace Formation_Ecommerce_11_2025.Core.Entities.Cart

{
    // Représente une ligne du panier (un produit avec sa quantité)
    public class CartDetails : BaseEntity
    {
        // ID de l'en-tête du panier (clé étrangère)
        [Required]
        [ForeignKey("CartHeader")]
        public Guid CartHeaderId { get; set; }

        // ID du produit ajouté au panier
        public Guid ProductId { get; set; }

        // Quantité du produit (entre 1 et 100)
        [Required]
        [Range(1, 100, ErrorMessage = "La quantité doit être entre 1 et 100.")]
        public int Count { get; set; }

        // Navigation vers le panier parent
        public CartHeader CartHeader { get; set; }

        // Navigation vers le produit
        public Formation_Ecommerce_11_2025.Core.Entities.Product.Product Product { get; set; }
    }
}
