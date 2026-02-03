using System.ComponentModel.DataAnnotations;

namespace Formation_Ecommerce_Client.Models.ViewModels.Coupons
{
    public class UpdateCouponViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Le code coupon est requis")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Le code doit contenir entre 3 et 50 caractères")]
        [Display(Name = "Code Coupon")]
        public string CouponCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le montant de réduction est requis")]
        [Range(0.01, 10000, ErrorMessage = "Le montant doit être entre 0.01 et 10000")]
        [Display(Name = "Montant de réduction")]
        public decimal DiscountAmount { get; set; }

        [Required(ErrorMessage = "Le montant minimum est requis")]
        [Range(0, 10000, ErrorMessage = "Le montant minimum doit être entre 0 et 10000")]
        [Display(Name = "Montant minimum d'achat")]
        public decimal MinimumAmount { get; set; }
    }
}
