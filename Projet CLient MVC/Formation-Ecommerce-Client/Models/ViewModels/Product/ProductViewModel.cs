using System.ComponentModel.DataAnnotations;

namespace Formation_Ecommerce_Client.Models.ViewModels.Products
{
    /// <summary>
    /// Modèle de présentation côté Client MVC : représente un produit tel qu'il est affiché/manipulé dans les vues Razor.
    /// </summary>
    /// <remarks>
    /// Points pédagogiques :
    /// - Dans l'architecture client/serveur, ce ViewModel est généralement alimenté par la réponse JSON de l'API.
    /// - Il ne correspond pas forcément 1:1 à l'entité métier côté serveur (Core) : on adapte les champs aux besoins d'affichage.
    /// - Les attributs <see cref="DisplayAttribute"/> et <see cref="DisplayFormatAttribute"/> concernent uniquement l'IHM.
    /// </remarks>
    public class ProductViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Nom du produit")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Prix")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal Price { get; set; }

        [Display(Name = "Catégorie")]
        public string CategoryName { get; set; } = string.Empty;

        public Guid? CategoryId { get; set; }
    }
}
