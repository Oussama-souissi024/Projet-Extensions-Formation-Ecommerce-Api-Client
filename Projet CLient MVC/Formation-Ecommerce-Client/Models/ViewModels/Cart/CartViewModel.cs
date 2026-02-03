using Formation_Ecommerce_Client.Models.ApiDtos.Cart;

namespace Formation_Ecommerce_Client.Models.ViewModels.Cart
{
    public class CartViewModel
    {
        public Guid CartHeaderId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public decimal OrderTotal { get; set; }
        public string? CouponCode { get; set; }
        public decimal Discount { get; set; }
        public List<CartDetailsDto> CartDetails { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public List<CartItemViewModel> Items { get; set; } = new();
    }

    public class AddToCartViewModel
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
