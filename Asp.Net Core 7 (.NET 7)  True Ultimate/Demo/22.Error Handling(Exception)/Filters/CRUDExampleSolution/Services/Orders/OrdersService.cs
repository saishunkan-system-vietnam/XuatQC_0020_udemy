using Entities;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helper;

namespace Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly ILogger<OrdersService> _logger;
        public OrdersService(IOrdersRepository ordersRepository, ILogger<OrdersService> logger)
        {
            _ordersRepository = ordersRepository;
            _logger = logger;
        }

        public async Task<OrderResponse> AddOrder(OrderAddRequest orderRequest)
        {
            _logger.LogInformation("AddOrder action method of OrdersService");
            _logger.LogDebug("AddOrder Request parameter: {OrderAddRequest}", orderRequest);

            if (orderRequest == null)
            {
                throw new ArgumentNullException(nameof(orderRequest));
            }

            ValidationHelper.ModelValidation(orderRequest);

            var order = orderRequest.ToOrder();

            order.OrderId = Guid.NewGuid();

            Order orderFromAdded = await _ordersRepository.AddOrder(order);

            OrderResponse orderResponse = orderFromAdded.ToOrderResponse();

            return orderResponse;
        }

        public async Task<bool> DeleteOrderByOrderId(Guid orderId)
        {
            _logger.LogInformation("DeleteOrderByOrderId action method of OrdersService");
            _logger.LogDebug("DeleteOrderByOrderId Request parameter: {orderId}", orderId);

            var result = await _ordersRepository.DeleteOrderByOrderId(orderId);
            return result;
        }

        public async Task<List<OrderResponse>> GetAllOrders()
        {
            _logger.LogInformation("GetAllOrders action method of OrdersService");

            var ordersFromRepository = await _ordersRepository.GetAllOrders();
            return ordersFromRepository.ToOrderResponseList();
        }

        public async Task<OrderResponse?> GetOrderByOrderId(Guid orderId)
        {
            _logger.LogInformation("GetOrderByOrderId action method of OrdersService");
            _logger.LogDebug("GetOrderByOrderId Request parameter: {orderId}", orderId);

            if (orderId == Guid.Empty)
            {
                throw new ArgumentNullException();
            }

            var orderFromRepository = await _ordersRepository.GetOrderByOrderId(orderId);
           
            return orderFromRepository?.ToOrderResponse();
        }

        public async Task<OrderResponse?> UpdateOrder(OrderUpdateRequest orderRequest)
        {
            _logger.LogInformation("UpdateOrder action method of OrdersService");
            _logger.LogDebug("UpdateOrder Request parameter: {OrderUpdateRequest}", orderRequest);

            if (orderRequest == null)
            {
                throw new ArgumentNullException(nameof(orderRequest));
            }
            if (orderRequest.OrderId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(orderRequest.OrderId));
            }

            ValidationHelper.ModelValidation(orderRequest);

            var order = orderRequest.ToOrder();


            var orderFromRepository = await _ordersRepository.UpdateOrder(order);

            return orderFromRepository?.ToOrderResponse();
        }
    }
}
