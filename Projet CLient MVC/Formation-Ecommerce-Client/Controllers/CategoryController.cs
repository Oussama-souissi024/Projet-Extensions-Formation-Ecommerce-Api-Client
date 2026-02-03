using Formation_Ecommerce_Client.Helpers;
using Formation_Ecommerce_Client.Models.ViewModels.Categories;
using Formation_Ecommerce_Client.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Formation_Ecommerce_Client.Controllers
{
    [AuthorizeApi] 
    public class CategoryController : Controller
    {
        private readonly ICategoryApiService _categoryService;

        public CategoryController(ICategoryApiService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> CategoryIndex()
        {
            try
            {
                var categories = await _categoryService.GetAllAsync();
                return View(categories);
            }
            catch
            {
               TempData["Error"] = "Erreur lors du chargement des catégories";
               return View(new List<CategoryViewModel>());
            }
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                try 
                {
                    await _categoryService.CreateAsync(model);
                    TempData["Success"] = "Catégorie créée !";
                    return RedirectToAction(nameof(CategoryIndex));
                }
                catch
                {
                    TempData["Error"] = "Erreur lors de la création";
                }
            }
            return View(model);
        }

        public async Task<IActionResult> EditCategory(Guid id)
        {
             try
            {
                var category = await _categoryService.GetByIdAsync(id);
                if(category == null) return NotFound();

                var model = new UpdateCategoryViewModel 
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                };
                return View(model);
            }
             catch
            {
                TempData["Error"] = "Erreur lors du chargement";
                 return RedirectToAction(nameof(CategoryIndex));
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(UpdateCategoryViewModel model)
        {
             if (ModelState.IsValid)
            {
                try 
                {
                    await _categoryService.UpdateAsync(model);
                    TempData["Success"] = "Catégorie modifiée !";
                    return RedirectToAction(nameof(CategoryIndex));
                }
                catch
                {
                    TempData["Error"] = "Erreur lors de la modification";
                }
            }
            return View(model);
        }

        public async Task<IActionResult> DeleteCategory(Guid id)
        {
             try
            {
                var category = await _categoryService.GetByIdAsync(id);
                if(category == null) return NotFound();
                return View(category);
            }
             catch
            {
                TempData["Error"] = "Erreur lors du chargement";
                 return RedirectToAction(nameof(CategoryIndex));
            }
        }

        [HttpPost, ActionName("DeleteCategory")]
        public async Task<IActionResult> DeleteCategoryConfirmed(Guid id)
        {
             try 
            {
                await _categoryService.DeleteAsync(id);
                TempData["Success"] = "Catégorie supprimée !";
            }
            catch
            {
                 TempData["Error"] = "Erreur lors de la suppression";
            }
            return RedirectToAction(nameof(CategoryIndex));
        }
    }
}
