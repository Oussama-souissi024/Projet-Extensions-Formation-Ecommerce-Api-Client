using Formation_Ecommerce_11_2025.Application.Categories.Dtos;
using Formation_Ecommerce_11_2025.Application.Categories.Interfaces;
using Formation_Ecommerce_11_2025.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Formation_Ecommerce_11_2025.API.Controllers
{
    /// <summary>
    /// Contrôleur Web API des catégories : expose le CRUD en endpoints REST (JSON) pour la ressource "Category".
    /// </summary>
    /// <remarks>
    /// Différences pédagogiques vs CategoryController MVC du projet monolithique :
    /// - Au lieu de retourner des vues (GET/POST), l'API renvoie des réponses HTTP standard (200/201/400/404).
    /// - Le client MVC séparé consomme ces endpoints et reste responsable du rendu UI.
    /// - La logique de catégories (validation, persistance) reste dans la couche Application/Infrastructure.
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Récupère toutes les catégories
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CategoryDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.ReadAllAsync();
            return Ok(new ApiResponse<IEnumerable<CategoryDto>>
            {
                Success = true,
                Data = categories
            });
        }

        /// <summary>
        /// Récupère une catégorie par son ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<CategoryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var category = await _categoryService.ReadByIdAsync(id);
                return Ok(new ApiResponse<CategoryDto>
                {
                    Success = true,
                    Data = category
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Catégorie non trouvée"
                });
            }
        }

        /// <summary>
        /// Crée une nouvelle catégorie
        /// </summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<CategoryDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto createCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m))
                    .ToList();

                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Validation échouée",
                    Errors = errors
                });
            }

            var result = await _categoryService.AddAsync(createCategoryDto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, new ApiResponse<CategoryDto>
            {
                Success = true,
                Data = result
            });
        }

        /// <summary>
        /// Met à jour une catégorie existante
        /// </summary>
        [HttpPut("{id:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryDto updateCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .Where(m => !string.IsNullOrWhiteSpace(m))
                    .ToList();

                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Validation échouée",
                    Errors = errors
                });
            }

            if (id != updateCategoryDto.Id)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "L'ID ne correspond pas"
                });
            }

            try
            {
                await _categoryService.ReadByIdAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Catégorie non trouvée"
                });
            }

            await _categoryService.UpdateAsync(updateCategoryDto);
            return Ok(new ApiResponse<object>
            {
                Success = true
            });
        }

        /// <summary>
        /// Supprime une catégorie
        /// </summary>
        [HttpDelete("{id:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _categoryService.ReadByIdAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Catégorie non trouvée"
                });
            }

            await _categoryService.DeleteAsync(id);
            return Ok(new ApiResponse<object>
            {
                Success = true
            });
        }
    }
}
