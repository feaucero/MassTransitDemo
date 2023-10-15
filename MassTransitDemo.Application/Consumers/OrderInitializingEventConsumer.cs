using MassTransit;
using MassTransitDemo.Application.Events;
using MassTransitDemo.Domain.Interfaces.Repositories;

namespace MassTransitDemo.Application.Consumers
{
    public class OrderInitializingEventConsumer : IConsumer<OrderInitializingEvent>
    {
        private readonly IOrderRepository _orderRepository;

        public OrderInitializingEventConsumer(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Task Consume(ConsumeContext<OrderInitializingEvent> context)
        {
            Console.WriteLine($"Iniciando pedido {context.CorrelationId.Value} e produto {context.Message.ProductId}");

            var orderId = _orderRepository.Add(
                new Entities.Order { 
                    CreatedAt = DateTime.Now, 
                    OrderStatus = Enums.OrderStatusType.Created,
                    ProductId = context.Message.ProductId,
                    Code = context.CorrelationId.Value}).Result;

            //context.RespondAsync(new OrderProcessInitiazationDto
            //{
            //    OrderId = context.Message.OrderId
            //});

            return Task.CompletedTask;
        }
    }
}
