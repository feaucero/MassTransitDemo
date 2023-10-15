using MassTransit;
using System.Runtime.CompilerServices;

namespace MassTransitDemo.Application.Events
{
    public class StockValidatingEvent
    {
        public Guid Code { get; set; }
        public int ProductId { get; set; }

        [ModuleInitializer]
        internal static void Init()
        {
            GlobalTopology.Send.UseCorrelationId<StockValidatingEvent>(x => x.Code);
        }
    }
}
