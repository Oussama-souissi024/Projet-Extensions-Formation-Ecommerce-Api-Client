using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formation_Ecommerce_11_2025.Core.Interfaces;
using Formation_Ecommerce_11_2025.Core.Interfaces.External.Mailing;
using Formation_Ecommerce_11_2025.Core.Interfaces.Repositories;
using Formation_Ecommerce_11_2025.Core.Interfaces.Repositories.Base;
using Formation_Ecommerce_11_2025.Infrastructure.External.Mailing;
using Formation_Ecommerce_11_2025.Infrastructure.Persistence;
using Formation_Ecommerce_11_2025.Infrastructure.Persistence.Repositories;
using Formation_Ecommerce_11_2025.Infrastructure.Persistence.Repositories.Base;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Formation_Ecommerce_11_2025.Infrastructure.Extension
{
    public static class InfrastructureRegistration
    {
        public static IServiceCollection AddInfrastructureRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            //Register EmailSender 
            services.AddScoped<IEmailSender, EmailSender>();

            //Register Repository
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICouponRepository, CouponRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<IFileHelper, FileHelper>();

            return services;
        }
    }
}
