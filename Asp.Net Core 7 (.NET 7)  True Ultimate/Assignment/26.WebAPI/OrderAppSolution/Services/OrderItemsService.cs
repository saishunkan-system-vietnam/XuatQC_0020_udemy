using Entities;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrderItemsService : IOrderItemsService
    {
        private readonly IOrderItemsRepository _orderItemsRepository;  
        public OrderItemsService(IOrderItemsRepository orderItemsRepository)
        {
            _orderItemsRepository = orderItemsRepository;
        }

        public async Task<OrderItemResponse> AddOrderItem(OrderItemAddRequest orderItemRequest)
        {
            OrderItem orderItemParam = orderItemRequest.ToOrderItem();
            OrderItem orderItemFromRepository = await _orderItemsRepository.AddOrderItem(orderItemParam);

            return orderItemFromRepository.ToOrderItemResponse();
        }

        public async Task<bool> DeleteOrderItemByOrderItemId(Guid orderItemId)
        {
            var result = await _orderItemsRepository.DeleteOrderItemByOrderItemId(orderItemId);
            return result;
        }

        public async Task<List<OrderItemResponse>> GetAllOrderItems()
        {
            
            var responseFromRepository = await _orderItemsRepository.GetAllOrderItems();

            return responseFromRepository.ToOrderItemResponseList();
        }

        public async Task<OrderItemResponse?> GetOrderItemByOrderItemId(Guid orderItemId)
        {
            var responseFromRepository = await _orderItemsRepository.GetOrderItemByOrderItemId(orderItemId);

            return responseFromRepository?.ToOrderItemResponse();
        }

        public async Task<List<OrderItemResponse>> GetOrderItemsByOrderId(Guid orderId)
        {
            var responseFromRepository = await _orderItemsRepository.GetOrderItemsByOrderId(orderId);

            return responseFromRepository.ToOrderItemResponseList();
        }

        public async Task<OrderItemResponse> UpdateOrderItem(OrderItemUpdateRequest orderItemRequest)
        {
            OrderItem orderItemParam = orderItemRequest.ToOrderItem();
            var responseFromRepository = await _orderItemsRepository.UpdateOrderItem(orderItemParam);

            return responseFromRepository.ToOrderItemResponse();
        }
    }
}
