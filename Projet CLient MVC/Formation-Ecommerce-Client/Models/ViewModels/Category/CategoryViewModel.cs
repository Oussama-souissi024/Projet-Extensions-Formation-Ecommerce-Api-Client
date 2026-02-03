using System.ComponentModel.DataAnnotations;

namespace Formation_Ecommerce_Client.Models.ViewModels.Categories
{
    /// <summary>
    /// Modèle de présentation côté Client MVC : représente une catégorie telle qu'affichée/manipulée dans l'IHM.
    /// </summary>
    /// <remarks>
    /// Points pédagogiques :
    /// - Dans l'architecture client/serveur, ce modèle est alimenté par les réponses JSON de l'API (CRUD catégories).
    /// - Il est orienté écran (UI) et peut évoluer indépendamment de l'entité serveur (Core).
    /// </remarks>
    public class CategoryViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
