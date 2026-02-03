using System.ComponentModel.DataAnnotations;

namespace Formation_Ecommerce_Client.Models.ViewModels.Products
{
    public class UpdateProductViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Le nom du produit est obligatoire")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 200 caractères")]
        [Display(Name = "Nom du produit")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Le prix est obligatoire")]
        [Range(0.01, 100000, ErrorMessage = "Le prix doit être compris entre 0,01 et 100 000")]
        [Display(Name = "Prix")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = true)]
        public decimal? Price { get; set; }

        [StringLength(1000, ErrorMessage = "La description ne peut pas dépasser 1000 caractères")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "URL de l'image")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Image existante")]
        public string? ExistingImageUrl { get; set; }

        [Range(0, 10000, ErrorMessage = "La quantité doit être comprise entre 0 et 10 000")]
        [Display(Name = "Quantité")]
        public int? Count { get; set; } = 1;

        [Display(Name = "Image du produit")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Nom de la catégorie")]
        public string? CategoryName { get; set; }

        [Required(ErrorMessage = "La catégorie est obligatoire")]
        [Display(Name = "Catégorie")]
        public Guid? CategoryId { get; set; }
    }
}

