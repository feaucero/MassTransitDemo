using MassTransit;
using MassTransitDemo.Application.Events;
using MassTransitDemo.Domain.Interfaces.Repositories;

namespace MassTransitDemo.Application.Consumers
{
    public class OrderFinishedEventConsumer : IConsumer<OrderFinishedEvent>
    {
        private readonly IOrderRepository _orderRepository;

        public OrderFinishedEventConsumer(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Task Consume(ConsumeContext<OrderFinishedEvent> context)
        {
            var orderId = _orderRepository.Update(
                new Entities.Order 
                { 
                    ClosedAt = DateTime.Now, 
                    OrderStatus = Enums.OrderStatusType.Closed, 
                    Code = context.CorrelationId.Value
                }).Result;

            Console.WriteLine($"Pedido {context.CorrelationId.Value} fechado");

            //context.RespondAsync(new OrderProcessInitiazationDto
            //{
            //    OrderId = context.Message.OrderId
            //});

            return Task.CompletedTask;
        }
    }
}
