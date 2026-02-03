using System.ComponentModel.DataAnnotations;

namespace Formation_Ecommerce_Client.Models.ViewModels.Products
{
    public class CreateProductViewModel
    {
        [Required(ErrorMessage = "Le nom du produit est obligatoire")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 200 caractères")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Le prix est obligatoire")]
        [Range(0.01, 100000, ErrorMessage = "Le prix doit être compris entre 0,01 et 100 000")]
        public decimal Price { get; set; }

        [StringLength(1000, ErrorMessage = "La description ne peut pas dépasser 1000 caractères")]
        public string Description { get; set; }

        [Display(Name = "URL de l'image")]
        public string? ImageUrl { get; set; }

        [Range(0, 10000, ErrorMessage = "La quantité doit être comprise entre 0 et 10 000")]
        public int? Count { get; set; } = 1;

        [Display(Name = "Image du produit")]
        public IFormFile ImageFile { get; set; }

        [Required(ErrorMessage = "La catégorie est obligatoire")]
        public Guid? CategoryId { get; set; }
    }
}
