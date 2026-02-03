using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Formation_Ecommerce_11_2025.Application.Orders.Dtos;
using Formation_Ecommerce_11_2025.Application.Orders.Interfaces;
using Formation_Ecommerce_11_2025.Core.Entities.Orders;
using Formation_Ecommerce_11_2025.Core.Interfaces.Repositories;

namespace Formation_Ecommerce_11_2025.Application.Orders.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderServices(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        // Create
        public async Task<OrderHeaderDto?> AddOrderHeaderAsync(OrderHeaderDto orderHeaderDto)
        {
            var orderHeader = _mapper.Map<OrderHeader>(orderHeaderDto);
            var added = await _orderRepository.AddOrderHeaderAsync(orderHeader);
            return _mapper.Map<OrderHeaderDto>(added);
        }

        public async Task<IEnumerable<OrderDetailsDto>?> AddOrderDetailsAsync(IEnumerable<OrderDetailsDto> orderDetailsDtoList)
        {
            var details = _mapper.Map<IEnumerable<OrderDetails>>(orderDetailsDtoList);
            var added = await _orderRepository.AddOrderDetailsAsync(details);
            return _mapper.Map<IEnumerable<OrderDetailsDto>>(added);
        }

        // Read
        public IEnumerable<OrderHeaderDto> GetAllOrdersAsync()
        {
            var orders = _orderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderHeaderDto>>(orders);
        }

        public IEnumerable<OrderHeaderDto> GetOrdersByUserIdAsync(string userId)
        {
            var orders = _orderRepository.GetAllByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<OrderHeaderDto>>(orders);
        }

        public async Task<OrderHeaderDto?> GetOrderByIdAsync(Guid orderHeaderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderHeaderId);
            return _mapper.Map<OrderHeaderDto>(order);
        }

        public async Task<OrderHeaderDto?> GetOrderWithDetailsByIdAsync(Guid orderHeaderId)
        {
            var order = await _orderRepository.GetByIdWithDetailsAsync(orderHeaderId);
            return _mapper.Map<OrderHeaderDto>(order);
        }

        // Update
        public async Task<bool?> UpdateOrderStatusAsync(Guid orderHeaderId, string newStatus)
        {
            return await _orderRepository.UpdateStatusAsync(orderHeaderId, newStatus);
        }

        public async Task<OrderHeaderDto?> UpdateOrderHeaderAsync(OrderHeaderDto orderHeaderDto)
        {
            var entity = _mapper.Map<OrderHeader>(orderHeaderDto);
            var updated = await _orderRepository.UpdateOrderHeaderAsync(entity);
            return _mapper.Map<OrderHeaderDto>(updated);
        }
    }
}
