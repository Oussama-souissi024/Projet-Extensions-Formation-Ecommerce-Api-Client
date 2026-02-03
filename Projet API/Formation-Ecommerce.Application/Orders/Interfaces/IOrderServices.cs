using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Formation_Ecommerce_11_2025.Application.Orders.Dtos;

namespace Formation_Ecommerce_11_2025.Application.Orders.Interfaces
{
    public interface IOrderServices
    {
        Task<OrderHeaderDto?> AddOrderHeaderAsync(OrderHeaderDto orderHeaderDto);
        Task<IEnumerable<OrderDetailsDto>?> AddOrderDetailsAsync(IEnumerable<OrderDetailsDto> orderDetailsDtoList);

        IEnumerable<OrderHeaderDto> GetAllOrdersAsync();
        IEnumerable<OrderHeaderDto> GetOrdersByUserIdAsync(string userId);
        Task<OrderHeaderDto?> GetOrderByIdAsync(Guid orderHeaderId);
        Task<OrderHeaderDto?> GetOrderWithDetailsByIdAsync(Guid orderHeaderId);

        Task<bool?> UpdateOrderStatusAsync(Guid orderHeaderId, string newStatus);
        Task<OrderHeaderDto?> UpdateOrderHeaderAsync(OrderHeaderDto orderHeaderDto);
    }
}
