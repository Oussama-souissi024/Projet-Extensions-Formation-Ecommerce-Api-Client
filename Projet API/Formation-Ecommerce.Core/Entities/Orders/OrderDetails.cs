using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formation_Ecommerce_11_2025.Core.Common;

namespace Formation_Ecommerce_11_2025.Core.Entities.Orders
{
    // Représente une ligne de commande (un produit avec sa quantité et son prix)
    public class OrderDetails : BaseEntity
    {

        // ID de l'en-tête de commande (clé étrangère)
        [Required]
        [ForeignKey("OrderHeader")]
        public Guid OrderHeaderId { get; set; }

        // ID du produit commandé
        [Required]
        [ForeignKey("Product")]
        public Guid ProductId { get; set; }

        // Prix du produit au moment de l'achat (conservé pour l'historique)
        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        // Quantité commandée
        public int Count { get; set; }

        // Navigation vers la commande parente
        public OrderHeader OrderHeader { get; set; }

        // Navigation vers le produit
        public Formation_Ecommerce_11_2025.Core.Entities.Product.Product Product { get; set; }
    }
}
