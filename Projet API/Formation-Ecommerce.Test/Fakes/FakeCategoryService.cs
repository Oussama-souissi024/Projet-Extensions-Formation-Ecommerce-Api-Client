using Formation_Ecommerce_11_2025.Application.Categories.Dtos;
using Formation_Ecommerce_11_2025.Application.Categories.Interfaces;

namespace Formation_Ecommerce_11_2025.Test.Fakes;

/// <summary>
/// Fake de <see cref="ICategoryService"/> pour les tests unitaires.
/// </summary>
/// <remarks>
/// Objectif pédagogique :
/// - Isoler les services qui dépendent de ICategoryService.
/// - Fournir des réponses configurables pour différents scénarios de test.
/// </remarks>
public class FakeCategoryService : ICategoryService
{
    private readonly Dictionary<Guid, CategoryDto> _store = new();

    /// <summary>
    /// Configurer l'ID à retourner par GetCategoryIdByNameAsync.
    /// Mettre à null pour simuler une catégorie introuvable.
    /// </summary>
    public Guid? CategoryIdByName { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Nom de la dernière catégorie recherchée.
    /// </summary>
    public string? LastSearchedCategoryName { get; private set; }

    public Task<CategoryDto> AddAsync(CreateCategoryDto categoryDto)
    {
        var dto = new CategoryDto
        {
            Id = Guid.NewGuid(),
            Name = categoryDto.Name,
            Description = categoryDto.Description
        };
        _store[dto.Id] = dto;
        return Task.FromResult(dto);
    }

    public Task<CategoryDto> ReadByIdAsync(Guid categoryId)
    {
        _store.TryGetValue(categoryId, out var dto);
        return Task.FromResult(dto!);
    }

    public Task<Guid?> GetCategoryIdByNameAsync(string categoryName)
    {
        LastSearchedCategoryName = categoryName;
        return Task.FromResult(CategoryIdByName);
    }

    public Task<IEnumerable<CategoryDto>> ReadAllAsync()
        => Task.FromResult<IEnumerable<CategoryDto>>(_store.Values.ToList());

    public Task UpdateAsync(UpdateCategoryDto updateCategoryDto)
    {
        if (_store.ContainsKey(updateCategoryDto.Id))
        {
            _store[updateCategoryDto.Id] = new CategoryDto
            {
                Id = updateCategoryDto.Id,
                Name = updateCategoryDto.Name,
                Description = updateCategoryDto.Description
            };
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        _store.Remove(id);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Ajoute une catégorie prédéfinie pour les tests.
    /// </summary>
    public void SeedCategory(CategoryDto category)
    {
        _store[category.Id] = category;
    }
}
