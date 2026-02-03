using AutoMapper;
using Formation_Ecommerce_11_2025.Application.Categories.Dtos;
using Formation_Ecommerce_11_2025.Core.Entities.Category;

namespace Formation_Ecommerce_11_2025.Application.Categories.Mapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            // Mapping between entity and DTO
            CreateMap<Category, CategoryDto>().ReverseMap();

            // Mapping for creation operation
            CreateMap<CreateCategoryDto, Category>();

            // Mapping for update operation
            CreateMap<UpdateCategoryDto, Category>();
        }
    }
}
