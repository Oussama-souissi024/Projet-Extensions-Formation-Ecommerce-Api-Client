using Formation_Ecommerce_11_2025.Core.Entities.Cart;
using Formation_Ecommerce_11_2025.Core.Entities.Category;
using Formation_Ecommerce_11_2025.Core.Entities.Coupon;
using Formation_Ecommerce_11_2025.Core.Entities.Identity;
using Formation_Ecommerce_11_2025.Core.Entities.Orders;
using Formation_Ecommerce_11_2025.Core.Entities.Product;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Formation_Ecommerce_11_2025.Infrastructure.Persistence
{
    /// <summary>
    /// DbContext EF Core (serveur) : point d'accès aux tables (DbSet) et intégration ASP.NET Identity.
    /// </summary>
    /// <remarks>
    /// Différences pédagogiques vs le projet monolithique MVC :
    /// - Dans l'extension, ce DbContext est réutilisé côté serveur (API) : la couche Data ne change pas.
    /// - Ce qui change, c'est la couche Présentation : au lieu de Controllers MVC + Views, on a des Controllers API.
    /// - L'héritage <see cref="IdentityDbContext{TUser}"/> apporte les tables Identity (users/roles/tokens) utilisées par JWT.
    /// </remarks>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }
        public DbSet<Category> categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<CartHeader> CartHeaders { get; set; }
        public DbSet<CartDetails> CartDetails { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
