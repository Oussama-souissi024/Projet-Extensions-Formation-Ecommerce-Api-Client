using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formation_Ecommerce_11_2025.Core.Common;
using Formation_Ecommerce_11_2025.Core.Entities.Cart;

namespace Formation_Ecommerce_11_2025.Core.Entities.Product
{
    // Représente un produit dans le catalogue e-commerce
    public class Product : BaseEntity
    {
        // Nom du produit affiché dans le catalogue
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        // Description détaillée du produit (caractéristiques, spécifications)
        public string Description { get; set; }

        // Prix unitaire du produit (stocké avec 2 décimales)
        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        // URL ou chemin de l'image du produit
        public string ImageUrl { get; set; }

        // ID de la catégorie à laquelle appartient ce produit (clé étrangère)
        [Required]
        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }

        // Navigation vers la catégorie du produit
        public Formation_Ecommerce_11_2025.Core.Entities.Category.Category Category { get; set; }

        // Collection des lignes de panier contenant ce produit
        public ICollection<CartDetails> CartDetails { get; set; }
    }
}
