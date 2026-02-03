using Formation_Ecommerce_11_2025.Core.Entities.Coupon;
using Formation_Ecommerce_11_2025.Core.Interfaces.Repositories;
using Formation_Ecommerce_11_2025.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Formation_Ecommerce_11_2025.Infrastructure.Persistence.Repositories
{
    public class CouponRepository : Repository<Coupon>, ICouponRepository
    {
        private readonly ApplicationDbContext _context;

        public CouponRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        // ---------------------------
        // CREATE
        // ---------------------------
        public async Task<Coupon> AddAsync(Coupon coupon)
        {
            try
            {
                await _context.Coupons.AddAsync(coupon);
                await _context.SaveChangesAsync();
                return coupon;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de l'ajout du coupon : {ex.Message}", ex);
            }
        }

        // ---------------------------
        // READ BY ID
        // ---------------------------
        public async Task<Coupon> ReadByIdAsync(Guid couponId)
        {
            try
            {
                var coupon = await _context.Coupons
                    .FirstOrDefaultAsync(c => c.Id == couponId);
                if (coupon == null)
                {
                    throw new KeyNotFoundException($"Coupon introuvable: {couponId}");
                }
                return coupon;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la lecture du coupon : {ex.Message}", ex);
            }
        }

        // ---------------------------
        // READ BY COUPON CODE
        // ---------------------------
        public async Task<Coupon> ReadByCouponCodeAsync(string couponCode)
        {
            try
            {
                var coupon = await _context.Coupons
                    .FirstOrDefaultAsync(c => c.CouponCode.ToLower() == couponCode.ToLower());
                if (coupon == null)
                {
                    throw new KeyNotFoundException($"Coupon introuvable pour le code: {couponCode}");
                }
                return coupon;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la recherche du coupon par code : {ex.Message}", ex);
            }
        }

        // ---------------------------
        // READ ALL
        // ---------------------------
        public async Task<IEnumerable<Coupon>> ReadAllAsync()
        {
            try
            {
                return await _context.Coupons.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération des coupons : {ex.Message}", ex);
            }
        }

        // ---------------------------
        // UPDATE
        // ---------------------------
        public async Task UpdateAsync(Coupon coupon)
        {
            try
            {
                _context.Coupons.Update(coupon);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la mise à jour du coupon : {ex.Message}", ex);
            }
        }

        // ---------------------------
        // DELETE
        // ---------------------------
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var coupon = await _context.Coupons.FindAsync(id);
                if (coupon != null)
                {
                    _context.Coupons.Remove(coupon);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la suppression du coupon : {ex.Message}", ex);
            }
        }
    }
}
