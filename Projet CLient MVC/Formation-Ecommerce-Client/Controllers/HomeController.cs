using Microsoft.AspNetCore.Mvc;
using Formation_Ecommerce_Client.Models;
using System.Diagnostics;
using Formation_Ecommerce_Client.Services.Implementations;
using Formation_Ecommerce_Client.Helpers;
using Formation_Ecommerce_Client.Models.ViewModels.Cart;

namespace Formation_Ecommerce_Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductApiService _productService;
        private readonly ICategoryApiService _categoryService;
        private readonly ICartApiService _cartService;

        public HomeController(IProductApiService productService, ICategoryApiService categoryService, ICartApiService cartService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            try 
            {
                // Affiche quelques produits phares sur l'accueil
                var products = await _productService.GetAllAsync();
                var homeProducts = products.Take(6).Select(p => new Formation_Ecommerce_Client.Models.ViewModels.Home.HomeProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl ?? string.Empty,
                    CategoryName = p.CategoryName
                });
                return View(homeProducts); 
            }
            catch 
            {
                return View(new List<Formation_Ecommerce_Client.Models.ViewModels.Home.HomeProductViewModel>());
            }
        }

        public async Task<IActionResult> ProductDetails(Guid productId)
        {
            try 
            {
                var product = await _productService.GetByIdAsync(productId);
                if (product == null) return NotFound();

                var homeProduct = new Formation_Ecommerce_Client.Models.ViewModels.Home.HomeProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description,
                    ImageUrl = product.ImageUrl ?? string.Empty,
                    CategoryName = product.CategoryName
                };

                return View(homeProduct);
            }
            catch 
            {
                TempData["Error"] = "Produit non trouvé.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [AuthorizeApi]
        public async Task<IActionResult> ProductDetails(Formation_Ecommerce_Client.Models.ViewModels.Home.HomeProductViewModel model, int quantity = 1)
        {
            try
            {
                await _cartService.AddToCartAsync(new Formation_Ecommerce_Client.Models.ViewModels.Cart.AddToCartViewModel 
                { 
                    ProductId = model.Id, 
                    Quantity = quantity 
                });
                TempData["Success"] = "Produit ajouté au panier !";
                return RedirectToAction("CartIndex", "Cart");
            }
            catch
            {
                TempData["Error"] = "Erreur lors de l'ajout au panier.";
                return RedirectToAction(nameof(Index));
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string? message = null)
        {
            ViewData["ErrorMessage"] = message;
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        public IActionResult ApiError()
        {
             return View();
        }
    }
}
