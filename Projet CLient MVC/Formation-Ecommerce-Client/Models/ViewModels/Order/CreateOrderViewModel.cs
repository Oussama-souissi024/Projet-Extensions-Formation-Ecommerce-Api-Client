namespace Formation_Ecommerce_Client.Models.ViewModels.Orders
{
    public class CreateOrderViewModel
    {
        public Guid UserId { get; set; }
        public string? PromoCode { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
