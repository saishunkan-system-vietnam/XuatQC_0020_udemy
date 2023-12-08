using Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;
        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        // GET: api/<OrdersController>
        [HttpGet]
        public async Task<ActionResult> GetAllOrders()
        {
            var orders = await _ordersService.GetAllOrders();

            return Ok(orders);
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderById(Guid id)
        {
            var order = await _ordersService.GetOrderByOrderId(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // POST api/<OrdersController>
        [HttpPost]
        public async Task<ActionResult> AddOrder(OrderAddRequest orderRequest)
        {

            var addedOrder = await _ordersService.AddOrder(orderRequest);

            return Ok(addedOrder);
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(Guid id, OrderUpdateRequest orderRequest)
        {
            if (id != orderRequest.OrderId)
            {
                return BadRequest();
            }
            var updatedOrder = await _ordersService.UpdateOrder(orderRequest);

            return Ok(updatedOrder);
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(Guid id)
        {
            var isDeleted = await _ordersService.DeleteOrderByOrderId(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
