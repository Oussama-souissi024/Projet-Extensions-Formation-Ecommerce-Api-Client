using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formation_Ecommerce_11_2025.Application.Products.Dtos;

namespace Formation_Ecommerce_11_2025.Application.Products.Interfaces
{
    public interface IProductServices
    {
        Task<ProductDto?> AddAsync(CreateProductDto createProductDto);
        Task<ProductDto> ReadByIdAsync(Guid productId);
        Task<IEnumerable<ProductDto>> ReadAllAsync();
        Task<IEnumerable<ProductDto>>? ReadProductsByCategoryName(string categoryName);
        Task UpdateAsync(UpdateProductDto updateProductDto);
        Task DeleteAsync(Guid id);
    }
}
