using MassTransit;
using MassTransitDemo.Application.Entities;
using MassTransitDemo.Application.Events;
using MassTransitDemo.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace MassTransitDemo.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
            _orderService = orderService;
        }

        [HttpPost] 
        public async Task<IActionResult> Post(long productId)
        {
            await _publishEndpoint.Publish<OrderInitializingEvent>(new { code = Guid.NewGuid(), productId });
            return new NoContentResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var order = await _orderService.Get(id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var orderList = await _orderService.List();
            return Ok(orderList);
        }
    }
}