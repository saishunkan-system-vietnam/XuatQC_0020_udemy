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
        private readonly ILogger<OrdersController> _logger;
        public OrdersController(IOrdersService ordersService, ILogger<OrdersController> logger)
        {
            _ordersService = ordersService;
            _logger = logger;
        }

        // GET: api/<OrdersController>
        [HttpGet]
        public async Task<ActionResult> GetAllOrders()
        {
            _logger.LogInformation("GetAllOrders method of OrdersController");

            var orders = await _ordersService.GetAllOrders();

            return Ok(orders);
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderById(Guid id)
        {
            _logger.LogInformation("GetOrderById action method of OrdersController");
            _logger.LogDebug("GetOrderById Request parameter: {orderid}", id);


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
            try
            {
                _logger.LogInformation("AddOrder action method of OrdersController");
                _logger.LogDebug("AddOrder Request parameter: {OrderAddRequest}", orderRequest);

                var addedOrder = await _ordersService.AddOrder(orderRequest);

                return Ok(addedOrder);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(Guid id, OrderUpdateRequest orderRequest)
        {
            _logger.LogInformation("UpdateOrder action method of OrdersController");
            _logger.LogDebug("UpdateOrder Request parameter: {OrderUpdateRequest}", orderRequest);

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
            _logger.LogInformation("DeleteOrder action method of OrdersController");
            _logger.LogDebug("DeleteOrder Request parameter: {OrderId}", id);

            var isDeleted = await _ordersService.DeleteOrderByOrderId(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
