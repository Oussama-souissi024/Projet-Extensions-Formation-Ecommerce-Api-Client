namespace Formation_Ecommerce_Client.Models.ViewModels.Categories
{
    /// <summary>
    /// Modèle de présentation côté Client MVC : données affichées lors de la confirmation de suppression d'une catégorie.
    /// </summary>
    /// <remarks>
    /// Le client affiche un récapitulatif et déclenche ensuite une requête HTTP vers l'API pour supprimer la ressource.
    /// </remarks>
    public class DeleteCategoryViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
