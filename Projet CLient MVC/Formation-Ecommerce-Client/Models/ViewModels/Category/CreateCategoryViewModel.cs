using System.ComponentModel.DataAnnotations;

namespace Formation_Ecommerce_Client.Models.ViewModels.Categories
{
    public class CreateCategoryViewModel
    {
        // Nom de la catégorie affiché dans le menu
        [Required(ErrorMessage = "Le nom de la catégorie est requis.")]
        [MaxLength(100, ErrorMessage = "Le nom de la catégorie ne peut pas dépasser 100 caractères.")]
        public string Name { get; set; }

        // Description optionnelle de la catégorie
        [Required(ErrorMessage = "La description de la catégorie est requis.")]
        [MaxLength(500, ErrorMessage = "La description ne peut pas dépasser 500 caractères.")]
        public string Description { get; set; }

    }
}
