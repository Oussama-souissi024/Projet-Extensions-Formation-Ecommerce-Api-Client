using Formation_Ecommerce_Client.Models.ViewModels.Products;
using Formation_Ecommerce_Client.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Formation_Ecommerce_Client.Controllers
{
    /// <summary>
    /// Contrôleur MVC côté Client : gère les pages produits (liste/création/édition/détails/suppression) en consommant l'API.
    /// </summary>
    /// <remarks>
    /// Points pédagogiques :
    /// - Dans l'architecture client/serveur, l'UI ne manipule pas directement EF Core : elle envoie des requêtes HTTP (JSON/multipart).
    /// - Les appels sont regroupés dans <see cref="IProductApiService"/> (produits) et <see cref="ICategoryApiService"/> (listes).
    /// - Les erreurs typiques à gérer côté UI : API indisponible, validation serveur, erreurs HTTP, etc.
    /// </remarks>
    public class ProductController : Controller
    {
        private readonly IProductApiService _productService;
        private readonly ICategoryApiService _categoryService;

        public ProductController(
            IProductApiService productService,
            ICategoryApiService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> ProductIndex()
        {
            try
            {
                var products = await _productService.GetAllAsync();
                return View(products);
            }
            catch (Exception)
            {
                TempData["Error"] = "Erreur de connexion à l'API";
                return View(new List<ProductViewModel>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(CreateProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View(model);
            }

            try
            {
                await _productService.CreateAsync(model);
                TempData["Success"] = "Produit créé avec succès!";
                return RedirectToAction(nameof(ProductIndex));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erreur: {ex.Message}";
                var categories = await _categoryService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(Guid id)
        {
            try 
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null) return NotFound();

                var updateViewModel = new UpdateProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    ExistingImageUrl = product.ImageUrl
                };

                var categories = await _categoryService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View(updateViewModel);
            }
            catch
            {
                 return RedirectToAction(nameof(ProductIndex));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(UpdateProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View(model);
            }

            try
            {
                await _productService.UpdateAsync(model.Id, model);
                TempData["Success"] = "Produit modifié avec succès!";
                return RedirectToAction(nameof(ProductIndex));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erreur: {ex.Message}";
                 var categories = await _categoryService.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                return View(model);
            }
        }

        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null) return NotFound();
                return View(product);
            }
            catch
            {
                TempData["Error"] = "Erreur lors du chargement du produit.";
                return RedirectToAction(nameof(ProductIndex));
            }
        }

        public async Task<IActionResult> DeleteProduct(Guid id)
        {
             try 
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null) return NotFound();
                return View(product);
            }
            catch
            {
                 return RedirectToAction(nameof(ProductIndex));
            }
        }

        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProductConfirmed(Guid id)
        {
            try
            {
                await _productService.DeleteAsync(id);
                TempData["Success"] = "Produit supprimé avec succès!";
                return RedirectToAction(nameof(ProductIndex));
            }
            catch (Exception)
            {
                TempData["Error"] = "Erreur lors de la suppression.";
                return RedirectToAction(nameof(ProductIndex));
            }
        }
    }
}
