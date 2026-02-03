using System;

namespace Formation_Ecommerce_Client.Models.ViewModels.Coupons
{
    /// <summary>
    /// Modèle de présentation côté Client MVC : représente un coupon (code, seuil, remise) affiché/manipulé dans l'IHM.
    /// </summary>
    /// <remarks>
    /// - Les coupons sont gérés côté API ; ce modèle sert à afficher les valeurs et à piloter les formulaires CRUD.
    /// - La validation métier (coupon valide, minimum, etc.) reste côté serveur.
    /// </remarks>
    public class CouponViewModel
    {
        public Guid Id { get; set; }
        public string CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal MinimumAmount { get; set; }
    }
}
