using Formation_Ecommerce_11_2025.Application.Coupons.Dtos;
using Formation_Ecommerce_11_2025.Application.Coupons.Interfaces;
using Formation_Ecommerce_11_2025.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Formation_Ecommerce_11_2025.API.Controllers
{
    /// <summary>
    /// Contrôleur Web API des coupons : endpoints JSON pour administrer les coupons et valider un code promo.
    /// </summary>
    /// <remarks>
    /// Différences pédagogiques vs CouponController MVC du projet monolithique :
    /// - Les actions sont sécurisées côté API via <see cref="AuthorizeAttribute"/> (ex: <c>Roles = "Admin"</c>).
    /// - Un endpoint de validation (<c>GET validate/{code}</c>) est utile pour le scénario "appliquer coupon" depuis un client.
    /// - La vue/UX est gérée côté Client MVC ; l'API reste la source de vérité (même si l'UI masque des boutons).
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    public class CouponsController : ControllerBase
    {
        private readonly ICouponService _couponService;

        public CouponsController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        /// <summary>
        /// Récupère tous les coupons
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CouponDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var coupons = await _couponService.ReadAllAsync();
            return Ok(new ApiResponse<IEnumerable<CouponDto>>
            {
                Success = true,
                Data = coupons
            });
        }

        /// <summary>
        /// Récupère un coupon par son ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<CouponDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var coupon = await _couponService.ReadByIdAsync(id);
            if (coupon == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Coupon non trouvé"
                });
            }

            return Ok(new ApiResponse<CouponDto>
            {
                Success = true,
                Data = coupon
            });
        }

        /// <summary>
        /// Valide un code coupon (accessible à tous les utilisateurs authentifiés)
        /// </summary>
        [HttpGet("validate/{code}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<CouponDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ValidateCoupon(string code)
        {
            var coupon = await _couponService.GetCouponByCodeAsync(code);
            if (coupon == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Coupon invalide ou expiré"
                });
            }

            return Ok(new ApiResponse<CouponDto>
            {
                Success = true,
                Data = coupon
            });
        }

        /// <summary>
        /// Crée un nouveau coupon
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<CouponDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CouponDto couponDto)
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

            var result = await _couponService.AddAsync(couponDto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, new ApiResponse<CouponDto>
            {
                Success = true,
                Data = result
            });
        }

        /// <summary>
        /// Met à jour un coupon existant
        /// </summary>
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCouponDto updateCouponDto)
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

            if (id != updateCouponDto.Id)
            {
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "L'ID ne correspond pas"
                });
            }

            var existing = await _couponService.ReadByIdAsync(id);
            if (existing == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Coupon non trouvé"
                });
            }

            await _couponService.UpdateAsync(updateCouponDto);
            return Ok(new ApiResponse<object>
            {
                Success = true
            });
        }

        /// <summary>
        /// Supprime un coupon
        /// </summary>
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _couponService.ReadByIdAsync(id);
            if (existing == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Coupon non trouvé"
                });
            }

            await _couponService.DeleteAsync(id);
            return Ok(new ApiResponse<object>
            {
                Success = true
            });
        }
    }
}
