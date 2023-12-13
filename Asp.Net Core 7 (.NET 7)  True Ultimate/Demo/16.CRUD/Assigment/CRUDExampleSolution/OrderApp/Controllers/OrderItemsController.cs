using Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderApp.Controllers
{
    [Route("api/orders/{orderId}/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemsService _OrderItemsService;
        private readonly ILogger<OrderItemsController> _logger;
        public OrderItemsController(IOrderItemsService OrderItemsService, ILogger<OrderItemsController> logger)
        {
            _OrderItemsService = OrderItemsService;
            _logger = logger;
        }

        // GET api/orders/{orderId}/<OrderItemsController>/5
        [HttpGet]
        public async Task<ActionResult<List<OrderItemResponse>>> GetOrderItemsByOrderId(Guid orderId)
        {
            _logger.LogInformation("GetOrderItemsByOrderId action method of OrderItemsController");
            _logger.LogDebug("GetOrderItemsByOrderId Request parameter: {orderid}", orderId);

            var orderItems = await _OrderItemsService.GetOrderItemsByOrderId(orderId);

            return Ok(orderItems);
        }

        // GET api/orders/{orderId}/<OrderItemsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderItemById(Guid id)
        {
            _logger.LogInformation("GetOrderItemById action method of OrderItemsController");
            _logger.LogDebug("GetOrderItemById Request parameter: {orderid}", id);

            var order = await _OrderItemsService.GetOrderItemByOrderItemId(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // POST api/orders/{orderId}/<OrderItemsController>
        [HttpPost]
        public async Task<ActionResult> AddOrderItem(OrderItemAddRequest orderItemRequest)
        {
            _logger.LogInformation("AddOrderItem action method of OrderItemsController");
            _logger.LogDebug("AddOrderItem Request parameter: {OrderItemAddRequest}", orderItemRequest);

            var addedOrderItem = await _OrderItemsService.AddOrderItem(orderItemRequest);

            return Ok(addedOrderItem);
        }

        // PUT api/orders/{orderId}/<OrderItemsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(Guid id, OrderItemUpdateRequest orderItemRequest)
        {
            _logger.LogInformation("UpdateOrder action method of OrderItemsController");
            _logger.LogDebug("UpdateOrder Request parameter: {OrderItemUpdateRequest}", orderItemRequest);

            if (id != orderItemRequest.OrderId)
            {
                return BadRequest();
            }
            var updatedOrder = await _OrderItemsService.UpdateOrderItem(orderItemRequest);

            return Ok(updatedOrder);
        }

        // DELETE api/orders/{orderId}/<OrderItemsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrderItem(Guid id)
        {
            _logger.LogInformation("DeleteOrderItem action method of OrderItemsController");
            _logger.LogDebug("DeleteOrderItem Request parameter: {OrderItemUpdateRequest}", id);

            var isDeleted = await _OrderItemsService.DeleteOrderItemByOrderItemId(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
