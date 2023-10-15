using MassTransit;

namespace MassTransitDemo.Orchestration
{
    public class OrderState : SagaStateMachineInstance, ISagaVersion
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }

        public DateTime OrderStartDate { get; set; }
        public long ProductId { get; set; }
        public int Version { get; set; }
    }
}
