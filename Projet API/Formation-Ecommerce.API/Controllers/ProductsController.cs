using Formation_Ecommerce_11_2025.Application.Products.Dtos;
using Formation_Ecommerce_11_2025.Application.Products.Interfaces;
using Formation_Ecommerce_11_2025.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Formation_Ecommerce_11_2025.API.Controllers
{
    /// <summary>
    /// Contrôleur Web API des produits : endpoints REST (JSON) pour lire et gérer le catalogue produits.
    /// </summary>
    /// <remarks>
    /// Différences pédagogiques vs ProductController MVC du projet monolithique :
    /// - Les opérations Create/Update acceptent du <c>multipart/form-data</c> (upload image) via <see cref="FromFormAttribute"/>.
    /// - Le contrôleur ne prépare plus de ViewModels : il renvoie des codes HTTP et des payloads JSON.
    /// - Le rendu (formulaires, listes) est déporté côté Client MVC, qui appelle ces endpoints.
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductServices _productServices;

        public ProductsController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        /// <summary>
        /// Récupère tous les produits
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<ProductDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productServices.ReadAllAsync();
            return Ok(new ApiResponse<IEnumerable<ProductDto>>
            {
                Success = true,
                Data = products
            });
        }

        /// <summary>
        /// Récupère un produit par son ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<ProductDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var product = await _productServices.ReadByIdAsync(id);
                return Ok(new ApiResponse<ProductDto>
                {
                    Success = true,
                    Data = product
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Produit non trouvé"
                });
            }
        }

        /// <summary>
        /// Crée un nouveau produit
        /// </summary>
        [HttpPost]
        [Authorize]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ApiResponse<ProductDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromForm] CreateProductDto createProductDto)
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

            var result = await _productServices.AddAsync(createProductDto);
            if (result == null)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Erreur lors de la création du produit"
                });
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, new ApiResponse<ProductDto>
            {
                Success = true,
                Data = result
            });
        }

        /// <summary>
        /// Met à jour un produit existant
        /// </summary>
        [HttpPut("{id:guid}")]
        [Authorize]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateProductDto updateProductDto)
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

            if (id != updateProductDto.Id)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "L'ID ne correspond pas"
                });
            }

            try
            {
                await _productServices.ReadByIdAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Produit non trouvé"
                });
            }

            await _productServices.UpdateAsync(updateProductDto);
            return Ok(new ApiResponse<object>
            {
                Success = true
            });
        }

        /// <summary>
        /// Supprime un produit
        /// </summary>
        [HttpDelete("{id:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _productServices.ReadByIdAsync(id);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Produit non trouvé"
                });
            }

            await _productServices.DeleteAsync(id);
            return Ok(new ApiResponse<object>
            {
                Success = true
            });
        }
    }
}
