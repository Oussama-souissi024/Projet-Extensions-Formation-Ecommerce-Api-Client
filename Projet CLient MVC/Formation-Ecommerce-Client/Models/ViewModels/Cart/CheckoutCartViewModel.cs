using System.Collections.Generic;
using Formation_Ecommerce_Client.Models.ApiDtos.Cart;

namespace Formation_Ecommerce_Client.Models.ViewModels.Cart
{
    /// <summary>
    /// Modèle de présentation côté Client MVC : panier utilisé dans l'écran de validation (checkout).
    /// </summary>
    /// <remarks>
    /// Points pédagogiques :
    /// - Le checkout affiche un instantané du panier récupéré via l'API, avant la création de commande.
    /// - Les DTO <see cref="CartHeaderDto"/> et <see cref="CartDetailsDto"/> permettent de rester aligné avec la structure API.
    /// </remarks>
    public class CheckoutCartViewModel
    {
        public CartHeaderDto CartHeader { get; set; } = new CartHeaderDto();
        public List<CartDetailsDto> CartDetails { get; set; } = new List<CartDetailsDto>();
    }
}
