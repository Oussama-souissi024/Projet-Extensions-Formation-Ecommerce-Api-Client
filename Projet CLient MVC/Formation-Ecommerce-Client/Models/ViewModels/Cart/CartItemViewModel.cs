namespace Formation_Ecommerce_Client.Models.ViewModels.Cart
{
    public class CartItemViewModel
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Count { get; set; }
        public decimal Total => Price * Quantity;
        public string? ImageUrl { get; set; }
    }
}
