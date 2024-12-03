using Entities;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helper;

namespace Services
{
    public class OrderItemsService : IOrderItemsService
    {
        private readonly IOrderItemsRepository _orderItemsRepository;

        private readonly ILogger<OrderItemsService> _logger;

        public OrderItemsService(IOrderItemsRepository orderItemsRepository, ILogger<OrderItemsService> logger)
        {
            _orderItemsRepository = orderItemsRepository;
            _logger = logger;
        }

        public async Task<OrderItemResponse> AddOrderItem(OrderItemAddRequest orderItemRequest)
        {
            _logger.LogInformation("AddOrderItem action method of OrderItemsService");
            _logger.LogDebug("AddOrderItem Request parameter: {OrderItemAddRequest}", orderItemRequest);

            if (orderItemRequest == null)
            {
                throw new ArgumentNullException(nameof(OrderItemAddRequest));
            }

            if (orderItemRequest.OrderId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(OrderItemAddRequest));
            }

            ValidationHelper.ModelValidation(orderItemRequest);

            OrderItem orderItemParam = orderItemRequest.ToOrderItem();
            OrderItem orderItemFromRepository = await _orderItemsRepository.AddOrderItem(orderItemParam);

            return orderItemFromRepository.ToOrderItemResponse();
        }

        public async Task<bool> DeleteOrderItemByOrderItemId(Guid orderItemId)
        {
            _logger.LogInformation("DeleteOrderItemByOrderItemId action method of OrderItemsService");
            _logger.LogDebug("DeleteOrderItemByOrderItemId Request parameter: {orderItemId}", orderItemId);

            if (orderItemId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(OrderItemAddRequest));
            }

            var result = await _orderItemsRepository.DeleteOrderItemByOrderItemId(orderItemId);
            return result;
        }

        public async Task<List<OrderItemResponse>> GetAllOrderItems()
        {

            _logger.LogInformation("GetAllOrderItems action method of OrderItemsService");

            var responseFromRepository = await _orderItemsRepository.GetAllOrderItems();

            return responseFromRepository.ToOrderItemResponseList();
        }

        public async Task<OrderItemResponse?> GetOrderItemByOrderItemId(Guid orderItemId)
        {
            _logger.LogInformation("GetOrderItemByOrderItemId action method of OrderItemsService");
            _logger.LogDebug("GetOrderItemByOrderItemId Request parameter: {orderItemId}", orderItemId);

            if (orderItemId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(OrderItemAddRequest));
            }

            var responseFromRepository = await _orderItemsRepository.GetOrderItemByOrderItemId(orderItemId);

            return responseFromRepository?.ToOrderItemResponse();
        }

        public async Task<List<OrderItemResponse>> GetOrderItemsByOrderId(Guid orderId)
        {
            _logger.LogInformation("GetOrderItemsByOrderId action method of OrderItemsService");
            _logger.LogDebug("GetOrderItemsByOrderId Request parameter: {orderId}", orderId);

            if (orderId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(OrderItemAddRequest));
            }

            var responseFromRepository = await _orderItemsRepository.GetOrderItemsByOrderId(orderId);

            return responseFromRepository.ToOrderItemResponseList();
        }

        public async Task<OrderItemResponse?> UpdateOrderItem(OrderItemUpdateRequest orderItemRequest)
        {
            _logger.LogInformation("UpdateOrderItem action method of OrderItemsService");
            _logger.LogDebug("UpdateOrderItem Request parameter: {OrderItemUpdateRequest}", orderItemRequest);

            if (orderItemRequest == null)
            {
                throw new ArgumentNullException(nameof(OrderItemAddRequest));
            }

            if (orderItemRequest.OrderItemId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(OrderItemAddRequest));
            }

            ValidationHelper.ModelValidation(orderItemRequest);

            // find order item by orderItemId
            var orderItemSearch = await _orderItemsRepository.GetOrderItemByOrderItemId(orderItemRequest.OrderItemId);

            if (orderItemSearch == null)
            {
                return null;
            }

            OrderItem orderItemParam = orderItemRequest.ToOrderItem();
            var responseFromRepository = await _orderItemsRepository.UpdateOrderItem(orderItemParam);

            return responseFromRepository.ToOrderItemResponse();
        }
    }
}
