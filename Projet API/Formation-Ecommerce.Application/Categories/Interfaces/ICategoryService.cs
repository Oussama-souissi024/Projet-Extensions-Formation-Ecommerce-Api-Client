using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formation_Ecommerce_11_2025.Application.Categories.Dtos;

namespace Formation_Ecommerce_11_2025.Application.Categories.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDto> AddAsync(CreateCategoryDto categoryDto);
        Task<CategoryDto> ReadByIdAsync(Guid categoryId);
        Task<Guid?> GetCategoryIdByNameAsync(string categoryName);
        Task<IEnumerable<CategoryDto>> ReadAllAsync();
        Task UpdateAsync(UpdateCategoryDto updateCategoryDto);
        Task DeleteAsync(Guid id);
    }
}
