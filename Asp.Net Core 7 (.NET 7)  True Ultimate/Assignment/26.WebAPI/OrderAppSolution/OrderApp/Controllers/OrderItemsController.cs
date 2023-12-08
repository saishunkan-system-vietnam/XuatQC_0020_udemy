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
        public OrderItemsController(IOrderItemsService OrderItemsService)
        {
            _OrderItemsService = OrderItemsService;
        }

        // GET: api/<OrderItemsController>
        [HttpGet]
        public async Task<ActionResult> GetAllOrderItems()
        {
            var OrderItems = await _OrderItemsService.GetAllOrderItems();

            return Ok(OrderItems);
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderItemResponse>>> GetOrderItemsByOrderId(Guid orderId)
        {

            var orderItems = await _OrderItemsService.GetOrderItemsByOrderId(orderId);

            return Ok(orderItems);
        }

        // GET api/<OrderItemsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderItemById(Guid id)
        {
            var order = await _OrderItemsService.GetOrderItemByOrderItemId(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // POST api/<OrderItemsController>
        [HttpPost]
        public async Task<ActionResult> AddOrderItem(OrderItemAddRequest orderItemRequest)
        {

            var addedOrderItem = await _OrderItemsService.AddOrderItem(orderItemRequest);

            return Ok(addedOrderItem);
        }

        // PUT api/<OrderItemsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(Guid id, OrderItemUpdateRequest orderItemRequest)
        {
            if (id != orderItemRequest.OrderId)
            {
                return BadRequest();
            }
            var updatedOrder = await _OrderItemsService.UpdateOrderItem(orderItemRequest);

            return Ok(updatedOrder);
        }

        // DELETE api/<OrderItemsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrderItem(Guid id)
        {
            var isDeleted = await _OrderItemsService.DeleteOrderItemByOrderItemId(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
