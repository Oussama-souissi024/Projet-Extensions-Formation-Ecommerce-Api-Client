using System.ComponentModel.DataAnnotations;

namespace Formation_Ecommerce_Client.Models.ViewModels.Products
{
    /// <summary>
    /// Modèle de présentation côté Client MVC : données affichées lors de la confirmation de suppression d'un produit.
    /// </summary>
    /// <remarks>
    /// Le client présente un récapitulatif puis déclenche une requête HTTP vers l'API pour supprimer le produit.
    /// </remarks>
    public class DeleteProductViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Nom du produit")]
        public string Name { get; set; }

        [Display(Name = "Prix")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal? Price { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Image")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Catégorie")]
        public string? CategoryName { get; set; }
    }
}
