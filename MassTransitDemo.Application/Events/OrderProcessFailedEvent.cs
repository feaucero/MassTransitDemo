using MassTransit;
using System.Runtime.CompilerServices;

namespace MassTransitDemo.Application.Events
{
    public class OrderProcessFailedEvent
    {
        public Guid Code { get; set; }
        public int ProductId { get; set; }

        [ModuleInitializer]
        internal static void Init()
        {
            GlobalTopology.Send.UseCorrelationId<OrderProcessFailedEvent>(x => x.Code);
        }
    }
}
