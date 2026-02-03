using Formation_Ecommerce_11_2025.Core.Entities.Category;
using Formation_Ecommerce_11_2025.Core.Entities.Product;
using Formation_Ecommerce_11_2025.Core.Interfaces.Repositories;

namespace Formation_Ecommerce_11_2025.Test.Fakes;

/// <summary>
/// Fake (en mémoire) de <see cref="IProductRepository" />.
/// </summary>
/// <remarks>
/// Objectif pédagogique :
/// - Simuler la persistance des produits sans EF Core.
/// - Fournir une implémentation simple pour les tests unitaires.
/// - Tracer le dernier produit mis à jour (LastUpdatedProduct).
/// </remarks>
public class FakeProductRepository : IProductRepository
{
    private readonly Dictionary<Guid, Product> _store = new();

    public Product? LastUpdatedProduct { get; private set; }

    public Task<Product> AddAsync(Product product)
    {
        if (product.Id == Guid.Empty)
            product.Id = Guid.NewGuid();

        product.Category ??= new Category
        {
            Id = product.CategoryId,
            Name = string.Empty,
            Description = string.Empty,
            Products = new List<Product>()
        };
        product.CartDetails ??= new List<Formation_Ecommerce_11_2025.Core.Entities.Cart.CartDetails>();

        _store[product.Id] = product;
        return Task.FromResult(product);
    }

    public Task<Product> ReadByIdAsync(Guid productId)
    {
        _store.TryGetValue(productId, out var product);
        return Task.FromResult(product!);
    }

    public Task<IEnumerable<Product>> ReadAllAsync()
        => Task.FromResult<IEnumerable<Product>>(_store.Values.ToList());

    public Task UpdateAsync(Product product)
    {
        _store[product.Id] = product;
        LastUpdatedProduct = product;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid productId)
    {
        _store.Remove(productId);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Product>> GetProductsByCategoryId(Guid categoryId)
    {
        var list = _store.Values.Where(p => p.CategoryId == categoryId).ToList();
        return Task.FromResult<IEnumerable<Product>>(list);
    }

    // Implémentation de IRepository<Product>
    public Task<Product> GetByIdAsync(Guid id) => ReadByIdAsync(id);
    
    public Task<IEnumerable<Product>> GetAllAsync() => ReadAllAsync();
    
    public Task Update(Product entity) => UpdateAsync(entity);
    
    public Task Remove(Product entity)
    {
        _store.Remove(entity.Id);
        return Task.CompletedTask;
    }
    
    public Task<int> SaveChangesAsync() => Task.FromResult(1);
}
