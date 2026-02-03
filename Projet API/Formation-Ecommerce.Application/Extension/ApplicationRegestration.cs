using Formation_Ecommerce_11_2025.Application.Athentication.Interfaces;
using Formation_Ecommerce_11_2025.Application.Athentication.Services;
using Formation_Ecommerce_11_2025.Application.Categories.Interfaces;
using Formation_Ecommerce_11_2025.Application.Categories.Mapping;
using Formation_Ecommerce_11_2025.Application.Categories.Services;
using Formation_Ecommerce_11_2025.Application.Products.Interfaces;
using Formation_Ecommerce_11_2025.Application.Products.Services;
using Formation_Ecommerce_11_2025.Application.Products.Mapping;
using Formation_Ecommerce_11_2025.Application.Coupons.Interfaces;
using Formation_Ecommerce_11_2025.Application.Coupons.Services;
using Formation_Ecommerce_11_2025.Application.Coupons.Mapping;
using Formation_Ecommerce_11_2025.Application.Cart.Interfaces;
using Formation_Ecommerce_11_2025.Application.Cart.Services;
using Formation_Ecommerce_11_2025.Application.Cart.Mapping;
using Formation_Ecommerce_11_2025.Application.Orders.Interfaces;
using Formation_Ecommerce_11_2025.Application.Orders.Mapping;
using Formation_Ecommerce_11_2025.Application.Orders.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Formation_Ecommerce_11_2025.Application.Extension
{
    public static class ApplicationRegistration
    {
        public static IServiceCollection AddApplicationRegistration(this IServiceCollection services)
        {
            //Register Services
            services.AddScoped<ICategoryService, CategoryServices>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IProductServices, ProductServices>();
            services.AddScoped<ICouponService, CouponServices>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderServices, OrderServices>();


            //Register AutoMapper Profiles
            services.AddAutoMapper(typeof(CategoryProfile));
            services.AddAutoMapper(typeof(ProductProfile));
            services.AddAutoMapper(typeof(CouponProfile));
            services.AddAutoMapper(typeof(CartMappingProfile));
            services.AddAutoMapper(typeof(OrderMappingProfile));

            return services;
        }
    }
}
