using Formation_Ecommerce_11_2025.Core.Entities.Product;
using Formation_Ecommerce_11_2025.Core.Interfaces.Repositories.Base;

namespace Formation_Ecommerce_11_2025.Core.Interfaces.Repositories
{
    public interface IProductRepository : IRepository<Formation_Ecommerce_11_2025.Core.Entities.Product.Product>
    {
        // Ajoute un nouveau produit à la base de données
        Task<Product> AddAsync(Product product);

        // Récupère un produit par son identifiant
        Task<Product> ReadByIdAsync(Guid productId);

        // Récupère tous les produits existants
        Task<IEnumerable<Product>> ReadAllAsync();

        // Met à jour un produit existant
        Task UpdateAsync(Product product);

        // Supprime un produit par son identifiant et retourne true si l'opération a réussi
        Task DeleteAsync(Guid productId);

        Task<IEnumerable<Product>> GetProductsByCategoryId(Guid categoryId);
    }
}
