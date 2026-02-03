namespace Formation_Ecommerce_Client.Models.ViewModels.Orders
{
    public class OrderItemViewModel
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal Total => Price * Count;
        public string? ImageUrl { get; set; }
    }
}
