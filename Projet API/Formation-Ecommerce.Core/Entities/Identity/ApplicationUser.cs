
using System.ComponentModel.DataAnnotations;
using Formation_Ecommerce_11_2025.Core.Entities.Cart;
using Formation_Ecommerce_11_2025.Core.Entities.Orders;
using Microsoft.AspNetCore.Identity;

namespace Formation_Ecommerce_11_2025.Core.Entities.Identity
{
    // Représente un utilisateur de l'application e-commerce
    // Étend IdentityUser pour ajouter des propriétés personnalisées (ZipCode, Address)
    // IdentityUser fournit déjà: Email, UserName, Password, PhoneNumber, etc.
    public class ApplicationUser : IdentityUser
    {
        // Code postal de l'utilisateur (requis, entre 1000 et 99999)
        [Range(1000, 99999, ErrorMessage = "Le code postal doit être entre 1000 et 99999.")]
        public int ZipCode { get; set; }

        // Adresse complète de l'utilisateur, utilisée pour la livraison
        [StringLength(255, ErrorMessage = "L'adresse ne peut pas dépasser 255 caractères.")]
        public string? Address { get; set; }

        // Collection des paniers de l'utilisateur (généralement un seul actif)
        public ICollection<CartHeader> CartHeaders { get; set; }

        // Collection des commandes passées par l'utilisateur (historique des achats)
        public ICollection<OrderHeader> OrderHeaders { get; set; }
    }
}
