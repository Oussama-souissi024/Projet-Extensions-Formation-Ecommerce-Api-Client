using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formation_Ecommerce_11_2025.Core.Interfaces.Repositories.Base
{
    // Interface générique pour les opérations CRUD (Create, Read, Update, Delete)
    // Pattern Repository: sépare la logique métier de l'accès aux données
    public interface IRepository<TEntity> where TEntity : class
    {
        // Ajoute une nouvelle entité et retourne l'entité avec son ID généré
        Task<TEntity> AddAsync(TEntity entity);

        // Récupère une entité par son ID (retourne null si introuvable)
        Task<TEntity> GetByIdAsync(Guid id);

        // Récupère toutes les entités (attention: peut retourner beaucoup de données)
        Task<IEnumerable<TEntity>> GetAllAsync();

        // Met à jour une entité existante
        Task Update(TEntity entity);

        // Supprime une entité
        Task Remove(TEntity entity);

        // Sauvegarde les changements et retourne le nombre d'enregistrements affectés
        Task<int> SaveChangesAsync();
    }
}
