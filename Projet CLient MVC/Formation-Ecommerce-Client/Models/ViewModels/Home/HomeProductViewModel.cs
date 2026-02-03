namespace Formation_Ecommerce_Client.Models.ViewModels.Home
{
    /// <summary>
    /// Modèle de présentation côté Client MVC : produit affiché sur l'accueil et la page de détail (IHM).
    /// </summary>
    /// <remarks>
    /// Points pédagogiques :
    /// - Les informations proviennent de l'API (liste produits, détail produit) puis sont adaptées à la vue.
    /// - La propriété <see cref="Count"/> sert à la saisie de quantité (ajout au panier), côté UI.
    /// - La sécurité et la persistance associées au panier/commandes restent côté API.
    /// </remarks>
    public class HomeProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string CategoryName { get; set; }
        public int Count { get; set; } = 1;
        public IFormFile? Image { get; set; }
    }
}
