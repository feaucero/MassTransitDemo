using MassTransit;
using MassTransitDemo.Application.Events;
using MassTransitDemo.Domain.Interfaces.Repositories;

namespace MassTransitDemo.Application.Consumers
{
    public class StockValidatingEventConsumer : IConsumer<StockValidatingEvent>
    {
        private readonly IOrderRepository _orderRepository;

        public StockValidatingEventConsumer(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Task Consume(ConsumeContext<StockValidatingEvent> context)
        {
            Console.WriteLine($"Validando estoque do pedido {context.CorrelationId.Value} e produto {context.Message.ProductId}");

            var changes = _orderRepository.Update(
                new Entities.Order 
                { 
                    OrderStatus = Enums.OrderStatusType.StockValidated, 
                    StockValidatedAt = DateTime.Now,
                    Code = context.CorrelationId.Value
                }).Result;

            Console.WriteLine($"Estoque validado para o pedido {context.CorrelationId.Value}");

            return Task.CompletedTask;
        }
    }
}
