using Formation_Ecommerce_11_2025.Application.Orders.Dtos;
using Formation_Ecommerce_11_2025.Application.Orders.Interfaces;
using Formation_Ecommerce_11_2025.API.Models;
using Formation_Ecommerce_11_2025.Core.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Formation_Ecommerce_11_2025.API.Controllers
{
    /// <summary>
    /// Contrôleur Web API des commandes : endpoints JSON pour consulter, créer et piloter le workflow de commande.
    /// </summary>
    /// <remarks>
    /// Différences pédagogiques vs OrderController MVC du projet monolithique :
    /// - Les actions de consultation retournent des listes/objets JSON au lieu de Views.
    /// - Les autorisations sont appliquées au niveau API (rôles Admin vs utilisateur) via <see cref="AuthorizeAttribute"/>.
    /// - Le filtrage par statut (Approved/ReadyForPickup/etc.) reste un besoin métier, mais l'UI est désormais déportée côté Client.
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderServices _orderServices;

        public OrdersController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

        // Action: OrderIndex (Mapped to GetMyOrders logic or GetAll logic?)
        // User asked for "OrderIndex". In MVC this returns a View. Here we return List.
        [HttpGet("OrderIndex")]
        public IActionResult OrderIndex()
        {
            var userId = GetUserId();
            var orders = _orderServices.GetOrdersByUserIdAsync(userId);
            return Ok(new ApiResponse<IEnumerable<OrderHeaderDto>>
            {
                Success = true,
                Data = orders
            });
        }

        // Action: OrderConfirmation
        [HttpGet("OrderConfirmation/{id:guid}")]
        public async Task<IActionResult> OrderConfirmation(Guid id)
        {
            var order = await _orderServices.GetOrderByIdAsync(id);
             if (order == null) return NotFound(new ApiResponse<object> { Success = false, Message = "Not Found" });
            return Ok(new ApiResponse<OrderHeaderDto> { Success = true, Data = order });
        }

        // Action: GetAll
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(string? status = null)
        {
            IEnumerable<OrderHeaderDto> list;
            if (User.IsInRole("Admin"))
            {
                list = _orderServices.GetAllOrdersAsync();
            }
            else
            {
                list = _orderServices.GetOrdersByUserIdAsync(GetUserId());
            }

            if (!string.IsNullOrEmpty(status))
            {
                 switch (status.ToLower())
                {
                    case "approved":
                        list = list.Where(u => u.Status == StaticDetails.Status_Approved);
                        break;
                    case "readyforpickup":
                        list = list.Where(u => u.Status == StaticDetails.Status_ReadyForPickup);
                        break;
                    case "cancelled":
                        list = list.Where(u => u.Status == StaticDetails.Status_Cancelled || u.Status == StaticDetails.Status_Refunded);
                        break;
                    default:
                        break;
                }
            }
            
            return Ok(new ApiResponse<IEnumerable<OrderHeaderDto>>
            {
                Success = true,
                Data = list.OrderByDescending(u => u.Id)
            });
        }

        // Action: OrderDetail
        [HttpGet("OrderDetail/{id:guid}")]
        public async Task<IActionResult> OrderDetail(Guid id)
        {
             var order = await _orderServices.GetOrderWithDetailsByIdAsync(id);
            if (order == null)
                return NotFound(new ApiResponse<object> { Success = false, Message = "Commande non trouvée" });
            
            var userId = GetUserId();
            if (order.UserId != userId && !User.IsInRole("Admin"))
                return Forbid();
            
            return Ok(new ApiResponse<OrderHeaderDto> { Success = true, Data = order });
        }

        // Action: ApproveOrder (Validate)
        [HttpPost("ApproveOrder")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveOrder([FromBody] Guid orderId)
        {
             await _orderServices.UpdateOrderStatusAsync(orderId, StaticDetails.Status_Approved);
             return Ok(new ApiResponse<object> { Success = true });
        }

        // Action: OrderReadyForPickup
        [HttpPost("OrderReadyForPickup")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> OrderReadyForPickup([FromBody] Guid orderId)
        {
             await _orderServices.UpdateOrderStatusAsync(orderId, StaticDetails.Status_ReadyForPickup);
             return Ok(new ApiResponse<object> { Success = true });
        }

        // Action: CompleteOrder
        [HttpPost("CompleteOrder")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CompleteOrder([FromBody] Guid orderId)
        {
             await _orderServices.UpdateOrderStatusAsync(orderId, StaticDetails.Status_Completed);
             return Ok(new ApiResponse<object> { Success = true });
        }

        // Action: CancelOrder
        [HttpPost("CancelOrder")]
        public async Task<IActionResult> CancelOrder([FromBody] Guid orderId)
        {
             var order = await _orderServices.GetOrderByIdAsync(orderId);
            if (order == null) return NotFound(new ApiResponse<object> { Success = false, Message = "Not Found" });

             var userId = GetUserId();
            if (order.UserId != userId && !User.IsInRole("Admin")) return Forbid();

            await _orderServices.UpdateOrderStatusAsync(orderId, StaticDetails.Status_Cancelled);
            return Ok(new ApiResponse<object> { Success = true });
        }

        // Action: Create (Required for functionality)
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] OrderHeaderDto orderHeaderDto)
        {
             if (!ModelState.IsValid) return BadRequest(new ApiResponse<object> { Success = false, Message = "Invalid Model" });

            orderHeaderDto.UserId = GetUserId();
            orderHeaderDto.Status = StaticDetails.Status_Pending;
            orderHeaderDto.OrderTime = DateTime.Now;

            var result = await _orderServices.AddOrderHeaderAsync(orderHeaderDto);
             // Details...
            if (orderHeaderDto.OrderDetails != null && orderHeaderDto.OrderDetails.Any())
            {
                foreach (var detail in orderHeaderDto.OrderDetails) detail.OrderHeaderId = result.Id;
                await _orderServices.AddOrderDetailsAsync(orderHeaderDto.OrderDetails);
            }
             return Ok(new ApiResponse<OrderHeaderDto> { Success = true, Data = result });
        }
    }
}
