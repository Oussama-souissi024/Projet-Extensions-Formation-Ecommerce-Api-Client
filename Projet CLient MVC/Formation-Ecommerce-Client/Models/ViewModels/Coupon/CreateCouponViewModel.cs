using System.ComponentModel.DataAnnotations;

namespace Formation_Ecommerce_Client.Models.ViewModels.Coupons
{
    /// <summary>
    /// Modèle de présentation côté Client MVC : données du formulaire de création d'un coupon envoyées à l'API.
    /// </summary>
    /// <remarks>
    /// - Les attributs de validation (DataAnnotations) améliorent l'UX côté client.
    /// - Les règles métier définitives (unicité du code, politique de remise) sont appliquées côté API.
    /// </remarks>
    public class CreateCouponViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string CouponCode { get; set; }

        [Required]
        [Range(0.01, 10000)]
        public decimal DiscountAmount { get; set; }

        [Required]
        [Range(0, 10000)]
        public decimal MinimumAmount { get; set; }
    }
}
