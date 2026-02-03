namespace Formation_Ecommerce_Client.Models.ViewModels.Categories
{
    /// <summary>
    /// Modèle de présentation côté Client MVC : payload de mise à jour d'une catégorie envoyé à l'API.
    /// </summary>
    /// <remarks>
    /// Ce type est utilisé par le service HTTP catégories pour appeler l'endpoint de mise à jour (ex: <c>PUT /categories/{id}</c>).
    /// </remarks>
    public class UpdateCategoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
