using Formation_Ecommerce_Client.Helpers;
using Formation_Ecommerce_Client.Models.ViewModels.Coupons;
using Formation_Ecommerce_Client.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Formation_Ecommerce_Client.Controllers
{
    [AuthorizeApi]
    public class CouponController : Controller
    {
        private readonly ICouponApiService _couponService;

        public CouponController(ICouponApiService couponService)
        {
            _couponService = couponService;
        }

        public async Task<IActionResult> CouponIndex()
        {
            var list = await _couponService.GetAllAsync();
            return View(list);
        }

        public IActionResult CreateCoupon()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCoupon(CreateCouponViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _couponService.CreateAsync(model);
                return RedirectToAction(nameof(CouponIndex));
            }
            return View(model);
        }

        public async Task<IActionResult> EditCoupon(Guid id)
        {
            try
            {
                var coupon = await _couponService.GetByIdAsync(id);
                if (coupon == null) return NotFound();
                
                var model = new UpdateCouponViewModel
                {
                    Id = coupon.Id,
                    CouponCode = coupon.CouponCode,
                    DiscountAmount = coupon.DiscountAmount,
                    MinimumAmount = coupon.MinimumAmount
                };
                return View(model);
            }
            catch
            {
                TempData["Error"] = "Erreur lors du chargement du coupon.";
                return RedirectToAction(nameof(CouponIndex));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCoupon(UpdateCouponViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _couponService.UpdateAsync(model.Id, model);
                TempData["Success"] = "Coupon modifié avec succès!";
                return RedirectToAction(nameof(CouponIndex));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erreur: {ex.Message}";
                return View(model);
            }
        }

        public async Task<IActionResult> DeleteCoupon(Guid id)
        {
            try
            {
                var coupon = await _couponService.GetByIdAsync(id);
                if (coupon == null) return NotFound();
                return View(coupon);
            }
            catch
            {
                return RedirectToAction(nameof(CouponIndex));
            }
        }

        [HttpPost, ActionName("DeleteCoupon")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCouponConfirmed(Guid id)
        {
            try
            {
                await _couponService.DeleteAsync(id);
                TempData["Success"] = "Coupon supprimé!";
            }
            catch
            {
                TempData["Error"] = "Erreur lors de la suppression.";
            }
            return RedirectToAction(nameof(CouponIndex));
        }
    }
}
