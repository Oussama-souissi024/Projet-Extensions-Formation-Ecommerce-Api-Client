using Formation_Ecommerce_Client.Models.ApiResponses;
using Formation_Ecommerce_Client.Models.ApiDtos.Cart;
using Formation_Ecommerce_Client.Models.ApiDtos.Orders;
using Formation_Ecommerce_Client.Models.ViewModels.Orders;
using System.Net.Http.Headers;

namespace Formation_Ecommerce_Client.Services.Implementations
{
    public interface IOrderApiService
    {
        Task<IEnumerable<OrderViewModel>> GetMyOrdersAsync();
        Task<IEnumerable<OrderViewModel>> GetAllOrdersAsync(string? status);
        Task<OrderViewModel> GetOrderByIdAsync(Guid id);
        Task<OrderViewModel> GetOrderConfirmationAsync(Guid id);
        Task<Guid> CreateOrderAsync(CreateOrderViewModel model);
        Task<bool> CancelOrderAsync(Guid orderId);
        Task<bool> OrderReadyForPickupAsync(Guid orderId);
        Task<bool> ApproveOrderAsync(Guid orderId);
        Task<bool> CompleteOrderAsync(Guid orderId);
    }

    public class OrderApiService : IOrderApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderApiService(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _httpContextAccessor = httpContextAccessor;

            var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<IEnumerable<OrderViewModel>> GetMyOrdersAsync()
        {
            var response = await _httpClient.GetAsync("orders/OrderIndex");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<OrderHeaderDto>>>();
            return (result?.Data ?? Enumerable.Empty<OrderHeaderDto>()).Select(MapToViewModel).ToList();
        }

        public async Task<IEnumerable<OrderViewModel>> GetAllOrdersAsync(string? status)
        {
            var url = "orders/GetAll";
            if (!string.IsNullOrEmpty(status)) url += $"?status={status}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<OrderHeaderDto>>>();
            return (result?.Data ?? Enumerable.Empty<OrderHeaderDto>()).Select(MapToViewModel).ToList();
        }

        public async Task<OrderViewModel> GetOrderByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"orders/OrderDetail/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<OrderHeaderDto>>();
            return MapToViewModel(result?.Data);
        }

        public async Task<OrderViewModel> GetOrderConfirmationAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"orders/OrderConfirmation/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<OrderHeaderDto>>();
            return MapToViewModel(result?.Data);
        }

        public async Task<Guid> CreateOrderAsync(CreateOrderViewModel model)
        {
            var cartResponse = await _httpClient.GetAsync("cart");
            cartResponse.EnsureSuccessStatusCode();
            var cartResult = await cartResponse.Content.ReadFromJsonAsync<ApiResponse<CartDto>>();
            var cart = cartResult?.Data;

            var details = (cart?.CartDetails ?? Enumerable.Empty<CartDetailsDto>())
                .Select(d =>
                {
                    var product = d.Product;
                    return new OrderDetailsDto
                    {
                        ProductId = d.ProductId,
                        ProductName = product?.Name ?? string.Empty,
                        ProductUrl = product?.ImageUrl ?? string.Empty,
                        Price = d.Price ?? product?.Price ?? 0m,
                        Count = d.Count
                    };
                })
                .ToList();

            var payload = new OrderHeaderDto
            {
                Name = model.Name,
                Phone = model.Phone,
                Email = model.Email,
                CouponCode = cart?.CartHeader?.CouponCode,
                Discount = cart?.CartHeader?.Discount ?? 0m,
                OrderTotal = cart?.CartHeader?.CartTotal,
                OrderDetails = details
            };

            var response = await _httpClient.PostAsJsonAsync("orders/Create", payload);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<OrderHeaderDto>>();
            return result?.Data?.Id ?? Guid.Empty;
        }

        public async Task<bool> CancelOrderAsync(Guid orderId)
        {
            var response = await _httpClient.PostAsJsonAsync($"orders/CancelOrder", orderId);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> OrderReadyForPickupAsync(Guid orderId)
        {
             var response = await _httpClient.PostAsJsonAsync($"orders/OrderReadyForPickup", orderId);
             return response.IsSuccessStatusCode;
        }

        public async Task<bool> ApproveOrderAsync(Guid orderId)
        {
             var response = await _httpClient.PostAsJsonAsync($"orders/ApproveOrder", orderId);
             return response.IsSuccessStatusCode;
        }

        public async Task<bool> CompleteOrderAsync(Guid orderId)
        {
             var response = await _httpClient.PostAsJsonAsync($"orders/CompleteOrder", orderId);
             return response.IsSuccessStatusCode;
        }

        private static OrderViewModel MapToViewModel(OrderHeaderDto? dto)
        {
            if (dto == null)
                return new OrderViewModel();

            var vm = new OrderViewModel
            {
                Id = dto.Id,
                OrderHeaderId = dto.Id,
                OrderTime = dto.OrderTime,
                OrderTotal = dto.OrderTotal ?? 0m,
                Status = dto.Status ?? string.Empty,
                Name = dto.Name ?? string.Empty,
                Email = dto.Email ?? string.Empty,
                Phone = dto.Phone ?? string.Empty,
                Discount = dto.Discount,
                OrderDetails = (dto.OrderDetails ?? Enumerable.Empty<OrderDetailsDto>())
                    .Select(d => new OrderItemViewModel
                    {
                        Id = d.Id,
                        ProductId = d.ProductId,
                        ProductName = d.ProductName,
                        Count = d.Count,
                        Price = d.Price,
                        ImageUrl = d.ProductUrl
                    })
                    .ToList()
            };

            if (!string.IsNullOrWhiteSpace(dto.UserId))
                vm.UserId = dto.UserId;

            return vm;
        }
    }
}
