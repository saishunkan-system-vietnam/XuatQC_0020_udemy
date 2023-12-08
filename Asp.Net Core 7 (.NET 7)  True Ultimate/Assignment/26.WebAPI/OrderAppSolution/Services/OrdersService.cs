using Entities;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helper;

namespace Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;

        public OrdersService(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        public async Task<OrderResponse> AddOrder(OrderAddRequest orderRequest)
        {
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
            var result = await _ordersRepository.DeleteOrderByOrderId(orderId);
            return result;
        }

        public async Task<List<OrderResponse>> GetAllOrders()
        {
            var ordersFromRepository = await _ordersRepository.GetAllOrders();
            return ordersFromRepository.ToOrderResponseList();
        }

        public async Task<OrderResponse?> GetOrderByOrderId(Guid orderId)
        {

            if (orderId == Guid.Empty)
            {
                throw new ArgumentNullException();
            }

            var orderFromRepository = await _ordersRepository.GetOrderByOrderId(orderId);
           
            return orderFromRepository?.ToOrderResponse();
        }

        public async Task<OrderResponse?> UpdateOrder(OrderUpdateRequest orderRequest)
        {
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
