using MassTransit;
using System.Runtime.CompilerServices;

namespace MassTransitDemo.Application.Events
{
    public class OrderFinalizatingEvent
    {
        public Guid Code { get; set; }
        public int ProductId { get; set; }

        [ModuleInitializer]
        internal static void Init()
        {
            GlobalTopology.Send.UseCorrelationId<OrderFinalizatingEvent>(x => x.Code);
        }
    }
}
