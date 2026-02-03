using System;
using System.Collections.Generic;
using Formation_Ecommerce_Client.Models.ApiDtos.Cart;

namespace Formation_Ecommerce_Client.Models.ViewModels.Cart
{
    /// <summary>
    /// Modèle de présentation côté Client MVC : agrège les données nécessaires à l'écran « panier ».
    /// </summary>
    /// <remarks>
    /// Points pédagogiques :
    /// - Dans l'architecture client/serveur, cet écran est alimenté par un appel HTTP à l'API panier.
    /// - On réutilise ici des DTO (<see cref="CartHeaderDto"/>, <see cref="CartDetailsDto"/>) qui reflètent le contrat API.
    /// - L'objectif est de servir l'IHM (Vue Razor) ; ce n'est pas une entité métier persistée.
    /// </remarks>
    public class CartIndexViewModel
    {
        public CartHeaderDto? CartHeader { get; set; }
        public List<CartDetailsDto> CartDetails { get; set;} = new();
    }
}

