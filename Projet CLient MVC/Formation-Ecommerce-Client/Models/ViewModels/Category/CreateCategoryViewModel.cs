using System.ComponentModel.DataAnnotations;

namespace Formation_Ecommerce_Client.Models.ViewModels.Categories
{
    /// <summary>
    /// Modèle de présentation côté Client MVC : données du formulaire de création de catégorie envoyées à l'API.
    /// </summary>
    /// <remarks>
    /// Dans l'architecture client/serveur, le client valide la saisie (DataAnnotations) puis délègue la création et la persistance à l'API.
    /// </remarks>
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
