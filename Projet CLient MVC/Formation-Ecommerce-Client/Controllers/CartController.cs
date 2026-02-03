using Formation_Ecommerce_Client.Helpers;
using Formation_Ecommerce_Client.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Formation_Ecommerce_Client.Models.ViewModels.Cart;

namespace Formation_Ecommerce_Client.Controllers
{
    /// <summary>
    /// Contrôleur MVC côté Client : gère les pages liées au panier et délègue les opérations à l'API via un service HTTP.
    /// </summary>
    /// <remarks>
    /// Différences pédagogiques vs le projet monolithique MVC :
    /// - Le contrôleur n'accède plus directement au DbContext / services Application ; il appelle <see cref="ICartApiService"/>.
    /// - L'attribut <see cref="AuthorizeApiAttribute"/> s'appuie sur le JWT stocké en session pour reconstituer l'identité côté UI.
    /// - Les erreurs peuvent provenir du réseau/HTTP : elles sont traduites ici en messages utilisateur (TempData) et redirections.
    /// </remarks>
    [AuthorizeApi]
    public class CartController : Controller
    {
        private readonly ICartApiService _cartService;

        public CartController(ICartApiService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> CartIndex()
        {
            try
            {
                var cart = await _cartService.GetCartAsync();
                return View(cart);
            }
            catch
            {
                TempData["Error"] = "Impossible de récupérer le panier.";
                return View(new CartViewModel());
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(Guid productId, int quantity = 1)
        {
            try
            {
                await _cartService.AddToCartAsync(new AddToCartViewModel { ProductId = productId, Quantity = quantity });
                TempData["Success"] = "Produit ajouté au panier !";
            }
            catch
            {
                TempData["Error"] = "Erreur lors de l'ajout au panier.";
            }

            return RedirectToAction(nameof(CartIndex));
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Guid itemId)
        {
            try
            {
                await _cartService.RemoveFromCartAsync(itemId);
                TempData["Success"] = "Produit retiré du panier.";
            }
            catch
            {
                TempData["Error"] = "Erreur lors de la suppression.";
            }
            return RedirectToAction(nameof(CartIndex));
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(Guid itemId, int quantity)
        {
             try
            {
                await _cartService.UpdateQuantityAsync(itemId, quantity);
                TempData["Success"] = "Quantité mise à jour.";
            }
            catch
            {
                TempData["Error"] = "Erreur lors de la mise à jour.";
            }
            return RedirectToAction(nameof(CartIndex));
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartViewModel model)
        {
            if(string.IsNullOrEmpty(model.CouponCode))
            {
                TempData["Error"] = "Veuillez entrer un code coupon";
                 return RedirectToAction(nameof(CartIndex));
            }

            try 
            {
                var success = await _cartService.ApplyCouponAsync(model.CouponCode);
                if(success) TempData["Success"] = "Coupon appliqué !";
                else TempData["Error"] = "Coupon invalide.";
            }
            catch
            {
                 TempData["Error"] = "Erreur lors de l'application du coupon";
            }
             return RedirectToAction(nameof(CartIndex));
        }

         [HttpPost]
        public async Task<IActionResult> RemoveCoupon()
        {
             try 
            {
                var success = await _cartService.RemoveCouponAsync();
                if(success) TempData["Success"] = "Coupon retiré !";
                else TempData["Error"] = "Impossible de retirer le coupon.";
            }
            catch
            {
                 TempData["Error"] = "Erreur.";
            }
             return RedirectToAction(nameof(CartIndex));
        }

        public IActionResult Checkout()
        {
            return RedirectToAction("Checkout", "Order");
        }
    }
}
