using Formation_Ecommerce_11_2025.Core.Entities.Category;
using Formation_Ecommerce_11_2025.Core.Entities.Product;
using Formation_Ecommerce_11_2025.Core.Interfaces.Repositories;

namespace Formation_Ecommerce_11_2025.Test.Fakes;

/// <summary>
/// Fake (en mémoire) de <see cref="ICategoryRepository" />.
/// </summary>
/// <remarks>
/// Objectif pédagogique :
/// - Simuler la persistance des catégories sans EF Core.
/// - Permettre les tests unitaires du CategoryService.
/// </remarks>
public class FakeCategoryRepository : ICategoryRepository
{
    private readonly Dictionary<Guid, Category> _store = new();

    public Task<Category> AddAsync(Category category)
    {
        if (category.Id == Guid.Empty)
            category.Id = Guid.NewGuid();

        category.Products ??= new List<Product>();
        _store[category.Id] = category;
        return Task.FromResult(category);
    }

    public Task<Category> ReadByIdAsync(Guid categoryId)
    {
        _store.TryGetValue(categoryId, out var category);
        return Task.FromResult(category!);
    }

    public Task<IEnumerable<Category>> ReadAllAsync()
        => Task.FromResult<IEnumerable<Category>>(_store.Values.ToList());

    public Task<Guid?> GetCategoryIdByCategoryNameAsync(string categoryName)
    {
        var category = _store.Values.FirstOrDefault(c => 
            c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(category?.Id);
    }

    public Task Update(Category category)
    {
        _store[category.Id] = category;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid categoryId)
    {
        _store.Remove(categoryId);
        return Task.CompletedTask;
    }

    // Implémentation de IRepository<Category>
    public Task<Category> GetByIdAsync(Guid id) => ReadByIdAsync(id);

    public Task<IEnumerable<Category>> GetAllAsync() => ReadAllAsync();

    public Task Remove(Category entity)
    {
        _store.Remove(entity.Id);
        return Task.CompletedTask;
    }

    public Task<int> SaveChangesAsync() => Task.FromResult(1);
}
