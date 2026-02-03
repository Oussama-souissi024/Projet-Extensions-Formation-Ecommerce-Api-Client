namespace Formation_Ecommerce_Client.Models.ViewModels.Home
{
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
