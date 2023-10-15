using MassTransit;
using MassTransitDemo.Application.Events;
using MassTransitDemo.Domain.Interfaces.Repositories;

namespace MassTransitDemo.Application.Consumers
{
    public class OrderFinalizatingEventConsumer : IConsumer<OrderFinalizatingEvent>
    {
        private readonly IOrderRepository _orderRepository;

        public OrderFinalizatingEventConsumer(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Task Consume(ConsumeContext<OrderFinalizatingEvent> context)
        {
            Console.WriteLine($"Finalizando pedido {context.CorrelationId.Value} e produto {context.Message.ProductId}");

            var orderId = _orderRepository.Update(
                new Entities.Order 
                { 
                    ConfirmedAt = DateTime.Now, 
                    OrderStatus = Enums.OrderStatusType.Confirmed, 
                    Code = context.CorrelationId.Value
                }).Result;

            Console.WriteLine($"Pedido {context.CorrelationId.Value} finalizado");
            return Task.CompletedTask;
        }
    }
}
