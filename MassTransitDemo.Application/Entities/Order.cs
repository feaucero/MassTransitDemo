using MassTransitDemo.Application.Enums;

namespace MassTransitDemo.Application.Entities
{
    public class Order
    {
        public long Id { get; set; }
        public Guid Code { get; set; }
        public OrderStatusType OrderStatus { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? StockValidatedAt { get; set; }
        public DateTime? PaymentValidatedAt { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public long ProductId { get; set; }
    }
}
