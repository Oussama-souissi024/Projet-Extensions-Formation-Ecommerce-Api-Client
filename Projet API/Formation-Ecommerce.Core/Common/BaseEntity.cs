using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formation_Ecommerce_11_2025.Core.Common
{
    // Classe de base dont héritent toutes les entités (Product, Category, Order, etc.)
    // Fournit des propriétés communes: identifiant unique et dates de suivi
    public abstract class BaseEntity
    {
        // Identifiant unique de l'entité (clé primaire, GUID)
        public Guid Id { get; set; }

        // Date de création de l'entité dans la base de données
        public DateTime CreatedDate { get; set; }

        // Date de dernière modification (null si jamais modifiée)
        public DateTime? LastModifiedDate { get; set; }
    }
}
