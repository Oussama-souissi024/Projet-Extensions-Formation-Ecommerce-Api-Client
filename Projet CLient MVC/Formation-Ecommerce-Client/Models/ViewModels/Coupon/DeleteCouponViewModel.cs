using System;

namespace Formation_Ecommerce_Client.Models.ViewModels.Coupons
{
    /// <summary>
    /// Modèle de présentation côté Client MVC : données affichées lors de la confirmation de suppression d'un coupon.
    /// </summary>
    /// <remarks>
    /// Le client affiche un récapitulatif puis déclenche une requête HTTP vers l'API pour supprimer le coupon.
    /// </remarks>
    public class DeleteCouponViewModel
    {
        public Guid Id { get; set; }
        public string CouponCode { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal MinimumAmount { get; set; }
    }
}
