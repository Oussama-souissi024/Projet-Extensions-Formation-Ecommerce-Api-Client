using Formation_Ecommerce_Client.Helpers;
using Formation_Ecommerce_Client.Models.ViewModels.Orders;
using Formation_Ecommerce_Client.Services.Implementations;
using Formation_Ecommerce_Client.Models.ApiDtos.Cart;
using Formation_Ecommerce_Client.Models.ViewModels.Cart;
using Microsoft.AspNetCore.Mvc;

namespace Formation_Ecommerce_Client.Controllers
{
    /// <summary>
    /// Contrôleur MVC côté Client : gère le parcours commande (checkout, confirmation, suivi) en déléguant à l'API.
    /// </summary>
    /// <remarks>
    /// Différences pédagogiques vs le projet monolithique MVC :
    /// - Dans le monolithe, le contrôleur pouvait composer directement des objets métier en mémoire + persister via la couche Application.
    /// - Ici, la création/gestion des commandes se fait via des endpoints API ; le client joue un rôle d'orchestrateur UI.
    /// - Le filtrage par rôle (<c>Admin</c>) s'appuie sur les claims reconstruits depuis le JWT (voir <see cref="AuthorizeApiAttribute"/>).
    /// </remarks>
    [AuthorizeApi]
    public class OrderController : Controller
    {
        private readonly IOrderApiService _orderService;
        private readonly ICartApiService _cartService;

        public OrderController(IOrderApiService orderService, ICartApiService cartService)
        {
            _orderService = orderService;
            _cartService = cartService;
        }

        public async Task<IActionResult> OrderIndex()
        {
            try
            {
                var orders = await _orderService.GetMyOrdersAsync();
                return View(orders);
            }
            catch
            {
                TempData["Error"] = "Erreur chargement commandes.";
                return View(new List<OrderViewModel>());
            }
        }

        public async Task<IActionResult> Checkout()
        {
            var cart = await _cartService.GetCartAsync();
            if (cart == null || !cart.Items.Any())
            {
                TempData["Error"] = "Votre panier est vide.";
                return RedirectToAction("CartIndex", "Cart");
            }
            
            var checkoutModel = new Formation_Ecommerce_Client.Models.ViewModels.Cart.CheckoutCartViewModel();
            checkoutModel.CartHeader.CartTotal = cart.TotalAmount;
            checkoutModel.CartHeader.CouponCode = cart.CouponCode;
            checkoutModel.CartHeader.Discount = cart.Discount;
            
            checkoutModel.CartDetails = cart.Items.Select(i => new Formation_Ecommerce_Client.Models.ApiDtos.Cart.CartDetailsDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                Count = i.Count,
                Product = new Formation_Ecommerce_Client.Models.ViewModels.Products.ProductViewModel
                {
                    Name = i.ProductName,
                    Price = i.Price,
                    ImageUrl = i.ImageUrl,
                    Description = i.Description
                }
            }).ToList();

            return View(checkoutModel);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CreateOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var cart = await _cartService.GetCartAsync();
                ViewBag.Cart = cart;
                return View(model);
            }

            try
            {
                var orderId = await _orderService.CreateOrderAsync(model);
                await _cartService.ClearCartAsync();
                
                // Strict Alignment: Redirect to OrderConfirmation
                return RedirectToAction(nameof(OrderConfirmation), new { id = orderId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Erreur lors de la commande : " + ex.Message;
                 var cart = await _cartService.GetCartAsync();
                ViewBag.Cart = cart;
                return View(model);
            }
        }

        public async Task<IActionResult> OrderConfirmation(Guid id)
        {
            var order = await _orderService.GetOrderConfirmationAsync(id);
            return View(order);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll(string status)
        {
            IEnumerable<OrderViewModel> orders;

            if (User.IsInRole("Admin"))
            {
                orders = await _orderService.GetAllOrdersAsync(status);
            }
            else
            {
                orders = await _orderService.GetMyOrdersAsync();
            }
            
            if (!User.IsInRole("Admin")) 
            {
                 switch (status)
                {
                    case "approved":
                        orders = orders.Where(u => u.Status == StaticDetails.Status_Approved);
                        break;
                    case "readyforpickup":
                        orders = orders.Where(u => u.Status == StaticDetails.Status_ReadyForPickup);
                        break;
                    case "cancelled":
                        orders = orders.Where(u => u.Status == StaticDetails.Status_Cancelled || u.Status == StaticDetails.Status_Refunded);
                        break;
                    default:
                        break;
                }
            }

            return Json(new { data = orders });
        }
        
        public async Task<IActionResult> OrderDetail(Guid id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                return View(order);
            }
            catch
            {
                return RedirectToAction(nameof(OrderIndex));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
             try
            {
                var success = await _orderService.CancelOrderAsync(orderId);
                if (success) TempData["Success"] = "Commande annulée.";
                else TempData["Error"] = "Impossible d'annuler la commande.";
            }
            catch
            {
                TempData["Error"] = "Erreur lors de l'annulation.";
            }
            return RedirectToAction(nameof(OrderDetail), new { id = orderId });
        }

        [HttpPost]
        public async Task<IActionResult> ValidateOrder(Guid orderId)
        {
            try
            {
                await _orderService.ApproveOrderAsync(orderId);
                TempData["Success"] = "Commande validée !";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Erreur validation: " + ex.Message;
            }
            return RedirectToAction(nameof(OrderDetail), new { id = orderId });
        }

        [HttpPost]
        public async Task<IActionResult> OrderReadyForPickup(Guid orderId)
        {
            try
            {
                await _orderService.OrderReadyForPickupAsync(orderId);
                 TempData["Success"] = "Statut mis à jour (Prête) !";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Erreur mise à jour: " + ex.Message;
            }
            return RedirectToAction(nameof(OrderDetail), new { id = orderId });
        }

        [HttpPost]
        public async Task<IActionResult> CompleteOrder(Guid orderId)
        {
            try
            {
                await _orderService.CompleteOrderAsync(orderId);
                TempData["Success"] = "Commande terminée !";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Erreur clôture: " + ex.Message;
            }
            return RedirectToAction(nameof(OrderDetail), new { id = orderId });
        }

        // ValidateOrder removed for strict alignment with API refactor.
        // Assuming Validation/Approval is handled by a different flow or implicit.
        /*
        [HttpPost]
        public async Task<IActionResult> ValidateOrder(Guid orderId)
        {
             // return RedirectToAction(nameof(OrderDetail), new { id = orderId });
        }
        */
    }
}
