using System.ComponentModel.DataAnnotations;
using Formation_Ecommerce_11_2025.Core.Common;

namespace Formation_Ecommerce_11_2025.Core.Entities.Category
{
    // Représente une catégorie de produits (ex: Électronique, Vêtements)
    public class Category : BaseEntity
    {
        // Nom de la catégorie affiché dans le menu
        [Required(ErrorMessage = "Le nom de la catégorie est requis.")]
        [MaxLength(100, ErrorMessage = "Le nom de la catégorie ne peut pas dépasser 100 caractères.")]
        public string Name { get; set; }

        // Description optionnelle de la catégorie
        [MaxLength(500, ErrorMessage = "La description ne peut pas dépasser 500 caractères.")]
        public string Description { get; set; }

        // Collection des produits appartenant à cette catégorie
        public ICollection<Formation_Ecommerce_11_2025.Core.Entities.Product.Product> Products { get; set; }
    }
}
