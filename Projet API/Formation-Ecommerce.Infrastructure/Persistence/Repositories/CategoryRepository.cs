using Formation_Ecommerce_11_2025.Core.Entities.Category;
using Formation_Ecommerce_11_2025.Core.Interfaces.Repositories;
using Formation_Ecommerce_11_2025.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Formation_Ecommerce_11_2025.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        // ---------------------------
        // CREATE
        // ---------------------------
        public async Task<Category> AddAsync(Category category)
        {
            try
            {
                await _context.categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de l'ajout de la catégorie : {ex.Message}", ex);
            }
        }

        // ---------------------------
        // READ BY ID
        // ---------------------------
        public async Task<Category> ReadByIdAsync(Guid categoryId)
        {
            try
            {
                var category = await _context.categories.FirstOrDefaultAsync(c => c.Id == categoryId);
                if (category == null)
                {
                    throw new KeyNotFoundException($"Catégorie introuvable: {categoryId}");
                }
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la lecture de la catégorie : {ex.Message}", ex);
            }
        }

        // ---------------------------
        // READ ALL
        // ---------------------------
        public async Task<IEnumerable<Category>> ReadAllAsync()
        {
            try
            {
                return await _context.categories
                    .OrderBy(c => c.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération des catégories : {ex.Message}", ex);
            }
        }

        // ---------------------------
        // GET CATEGORY ID BY NAME
        // ---------------------------
        public async Task<Guid?> GetCategoryIdByCategoryNameAsync(string categoryName)
        {
            try
            {
                var category = await _context.categories
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == categoryName.ToLower());

                return category?.Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la recherche du nom de catégorie : {ex.Message}", ex);
            }
        }

        // ---------------------------
        // UPDATE
        // ---------------------------
        public async Task Update(Category categoryToUpdate)
        {
            try
            {
                _context.categories.Update(categoryToUpdate);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la mise à jour de la catégorie : {ex.Message}", ex);
            }
        }

        // ---------------------------
        // DELETE
        // ---------------------------
        public async Task DeleteAsync(Guid categoryId)
        {
            try
            {
                var category = await _context.categories.FindAsync(categoryId);

                if (category != null)
                {
                    _context.categories.Remove(category);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la suppression de la catégorie : {ex.Message}", ex);
            }
        }
    }
}
