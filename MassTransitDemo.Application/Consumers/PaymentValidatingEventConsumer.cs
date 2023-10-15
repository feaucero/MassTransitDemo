using MassTransit;
using MassTransitDemo.Application.Events;
using MassTransitDemo.Domain.Interfaces.Repositories;

namespace MassTransitDemo.Application.Consumers
{
    public class PaymentValidatingEventConsumer : IConsumer<PaymentValidatingEvent>
    {
        private readonly IOrderRepository _orderRepository;

        public PaymentValidatingEventConsumer(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Task Consume(ConsumeContext<PaymentValidatingEvent> context)
        {
            Console.WriteLine($"Validando pagamento do pedido {context.CorrelationId.Value} e produto {context.Message.ProductId}");

            var changes = _orderRepository.Update(
                new Entities.Order
                {
                    OrderStatus = Enums.OrderStatusType.PaymentValidate,
                    PaymentValidatedAt = DateTime.Now,
                    Code = context.CorrelationId.Value
                }).Result;

            Console.WriteLine($"Pagamento validado para o pedido {context.CorrelationId.Value}");

            //context.RespondAsync(new OrderProcessInitiazationDto
            //{
            //    OrderId = context.Message.OrderId
            //});

            return Task.CompletedTask;
        }
    }
}
