using MassTransit;
using MassTransitDemo.Application.Events;
using MassTransitDemo.Domain.Interfaces.Repositories;

namespace MassTransitDemo.Application.Consumers
{
    public class OrderProcessFailedEventConsumer : IConsumer<OrderProcessFailedEvent>
    {
        private readonly IOrderRepository _orderRepository;

        public OrderProcessFailedEventConsumer(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Task Consume(ConsumeContext<OrderProcessFailedEvent> context)
        {
            Console.WriteLine($"Iniciando pedido {context.CorrelationId.Value}");

            var orderId = _orderRepository.Add(
                new Entities.Order { 
                    CreatedAt = DateTime.Now, 
                    OrderStatus = Enums.OrderStatusType.Created, 
                    Code = context.CorrelationId.Value}).Result;

            Console.WriteLine($"Pedido {context.CorrelationId.Value} iniciado");

            //context.RespondAsync(new OrderProcessInitiazationDto
            //{
            //    OrderId = context.Message.OrderId
            //});

            return Task.CompletedTask;
        }
    }
}
