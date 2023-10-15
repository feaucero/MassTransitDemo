using MassTransit;
using System.Runtime.CompilerServices;

namespace MassTransitDemo.Application.Events
{
    public class PaymentValidatingEvent
    {
        public Guid Code { get; set; }
        public int ProductId { get; set; }

        [ModuleInitializer]
        internal static void Init()
        {
            GlobalTopology.Send.UseCorrelationId<PaymentValidatingEvent>(x => x.Code);
        }
    }
}
