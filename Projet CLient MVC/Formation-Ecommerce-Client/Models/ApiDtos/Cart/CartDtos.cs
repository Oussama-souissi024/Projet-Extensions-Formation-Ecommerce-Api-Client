using Formation_Ecommerce_Client.Models.ViewModels.Products;

namespace Formation_Ecommerce_Client.Models.ApiDtos.Cart
{
    public class CartDto
    {
        public CartHeaderDto CartHeader { get; set; } = new CartHeaderDto();
        public IEnumerable<CartDetailsDto> CartDetails { get; set; } = new List<CartDetailsDto>();
    }

    public class CartHeaderDto
    {
        public Guid Id { get; set; }
        public string UserID { get; set; } = string.Empty;
        public string? CouponCode { get; set; }

        public decimal? CartTotal { get; set; }
        public decimal? Discount { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }

    public class CartDetailsDto
    {
        public Guid Id { get; set; }
        public Guid CartHeaderId { get; set; }
        public Guid ProductId { get; set; }
        public int Count { get; set; }

        public ProductViewModel? Product { get; set; }
        public decimal? Price { get; set; }
    }

    public class ApplyCouponRequest
    {
        public string CouponCode { get; set; } = "";
    }
}
