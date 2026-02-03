using System.ComponentModel.DataAnnotations;

namespace Formation_Ecommerce_Client.Models.ViewModels.Orders
{
    /// <summary>
    /// Modèle de vue pour les détails d'une commande (lignes de commande)
    /// </summary>
    public class OrderDetailsViewModel
    {
        public Guid Id { get; set; }
        
        [Display(Name = "ID Commande")]
        public Guid OrderHeaderId { get; set; }
        
        [Display(Name = "ID Produit")]
        public Guid ProductId { get; set; }
        
        [Display(Name = "Nom du produit")]
        public string ProductName { get; set; }
        
        [Display(Name = "Prix")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Price { get; set; }
        
        [Display(Name = "Quantité")]
        public int Count { get; set; }
        
        // Propriété calculée pour le total de la ligne
        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalPrice => Price * Count;
        
        // Image du produit (optionnelle)
        public string ImageUrl { get; set; }
    }
}
