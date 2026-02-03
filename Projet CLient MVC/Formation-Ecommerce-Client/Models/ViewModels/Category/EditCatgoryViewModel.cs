namespace Formation_Ecommerce_Client.Models.ViewModels.Categories
{
    /// <summary>
    /// Modèle de présentation côté Client MVC : données affichées/éditées lors de la modification d'une catégorie.
    /// </summary>
    /// <remarks>
    /// Dans l'architecture client/serveur, l'enregistrement des modifications est réalisé via une requête HTTP (PUT) vers l'API.
    /// </remarks>
    public class EditCatgoryViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
