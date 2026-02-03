using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formation_Ecommerce_11_2025.Core.Common;
using Formation_Ecommerce_11_2025.Core.Entities.Identity;

namespace Formation_Ecommerce_11_2025.Core.Entities.Orders
{
    // Représente l'en-tête d'une commande (infos client, montants, statut)
    public class OrderHeader : BaseEntity
    {
        // ID de l'utilisateur qui a passé la commande (clé étrangère)
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        // Code du coupon de réduction appliqué (optionnel)
        public string? CouponCode { get; set; }

        // Montant de la réduction appliquée
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Discount { get; set; }

        // Montant total final de la commande (après réduction)
        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? OrderTotal { get; set; }

        // Nom complet du client
        public string Name { get; set; }

        // Numéro de téléphone du client
        public string Phone { get; set; }

        // Adresse email du client
        public string Email { get; set; }

        // Date et heure de création de la commande
        [Required]
        public DateTime OrderTime { get; set; }

        // Statut actuel de la commande (Pending, Approved, ReadyForPickup, Completed, etc.)
        // Utiliser les constantes de StaticDetails.Status_*
        public string Status { get; set; }

        // Navigation vers l'utilisateur qui a passé la commande
        public ApplicationUser User { get; set; }

        // Collection des lignes de la commande (articles commandés)
        public ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
