using AutoMapper;
using Formation_Ecommerce_11_2025.Application.Categories.Dtos;
using Formation_Ecommerce_11_2025.Application.Categories.Interfaces;
using Formation_Ecommerce_11_2025.Core.Entities.Category;
using Formation_Ecommerce_11_2025.Core.Interfaces.Repositories;

namespace Formation_Ecommerce_11_2025.Application.Categories.Services
{
    public class CategoryServices : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryServices(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<CategoryDto> AddAsync(CreateCategoryDto categoryDto)
        {
            // Map the DTO to entity
            var category = _mapper.Map<Category>(categoryDto);

            // Add the entity using repository
            var addedCategory = await _categoryRepository.AddAsync(category);

            // Return the result mapped to DTO
            return _mapper.Map<CategoryDto>(addedCategory);
        }

        public async Task<CategoryDto> ReadByIdAsync(Guid categoryId)
        {
            var category = await _categoryRepository.ReadByIdAsync(categoryId);
            if (category == null)
            {
                throw new KeyNotFoundException("Category Not Found");
            }
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<Guid?> GetCategoryIdByNameAsync(string categoryName)
        {
            var categoryId = await _categoryRepository.GetCategoryIdByCategoryNameAsync(categoryName);
            if (categoryId == null)
            {
                throw new KeyNotFoundException("Category Not Found");
            }
            return categoryId;
        }

        public async Task<IEnumerable<CategoryDto>> ReadAllAsync()
        {
            var categoryList = await _categoryRepository.ReadAllAsync();
            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categoryList);
            return categoryDtos;
        }

        public async Task UpdateAsync(UpdateCategoryDto updateCategoryDto)
        {
            // Map the DTO to entity
            var categoryToUpdate = _mapper.Map<Category>(updateCategoryDto);

            // Update the entity in the repository - la vérification d'existence se fera dans le repository
            await _categoryRepository.Update(categoryToUpdate);
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                // Delegate the entire deletion process to the repository
                // which will handle both checking if the entity exists and removing it
                await _categoryRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                // If there's an issue in the repository layer, we can still throw an appropriate exception
                throw new Exception($"Failed to delete category: {ex.Message}", ex);
            }
        }
    }
}

