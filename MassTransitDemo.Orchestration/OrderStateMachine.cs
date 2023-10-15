using MassTransit;
using MassTransitDemo.Application.Events;

namespace MassTransitDemo.Orchestration
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        public State OrderInitializedState { get; }
        public State OrderInitializedFaultedState { get; }

        public State StockValidatedState { get; }
        public State StockValidatedFaultedState { get; }

        public State PaymentValidatedState { get; }
        public State PaymentValidatedFaultedState { get; }

        public State OrderFinishedState { get; }
        public State OrderFinishedFaultedState { get; }

        public State OrderFailedState { get; }


        public Event<OrderInitializingEvent> OrderInitializingEvent { get; }
        public Event<Fault<OrderInitializingEvent>> OrderInitializingFaultEvent { get; }

        public Event<StockValidatingEvent> StockValidatingEvent { get; }
        public Event<Fault<StockValidatingEvent>> StockValidatingFaultEvent { get; }

        public Event<PaymentValidatingEvent> PaymentValidatingEvent { get; }
        public Event<Fault<PaymentValidatingEvent>> PaymentValidatingFaultEvent { get; }

        public Event<OrderFinalizatingEvent> OrderFinalizatingEvent { get; }
        public Event<Fault<OrderFinalizatingEvent>> OrderFinalizatingFaultEvent { get; }

        public Event<OrderFinishedEvent> OrderFinishedEvent { get; }
        public Event<Fault<OrderFinishedEvent>> OrderFinishedFaultEvent { get; }

        public Event<OrderProcessFailedEvent> OrderProcessFailedEvent { get; }


        public OrderStateMachine()
        {
            Event(() => OrderInitializingEvent);
            Event(() => OrderInitializingFaultEvent,
                x => x.CorrelateById(context => context.InitiatorId ?? context.Message.Message.Code));

            Event(() => StockValidatingEvent);
            Event(() => StockValidatingFaultEvent,
                x => x.CorrelateById(context => context.InitiatorId ?? context.Message.Message.Code));

            Event(() => PaymentValidatingEvent);
            Event(() => PaymentValidatingFaultEvent,
                x => x.CorrelateById(context => context.InitiatorId ?? context.Message.Message.Code));

            Event(() => OrderFinalizatingEvent);
            Event(() => OrderFinalizatingFaultEvent,
                x => x.CorrelateById(context => context.InitiatorId ?? context.Message.Message.Code));

            Event(() => OrderFinishedEvent);
            Event(() => OrderFinishedFaultEvent,
                x => x.CorrelateById(context => context.InitiatorId ?? context.Message.Message.Code));

            Event(() => OrderProcessFailedEvent);


            InstanceState(x => x.CurrentState);

            During(Initial,
                When(OrderInitializingEvent)
                    .Then(context => { Console.WriteLine($"1 - During Initial When OrderInitializingEvent para {context.Saga.CorrelationId} "); })
                    .Then(x => x.Saga.OrderStartDate = DateTime.Now)
                    .Then(x => x.Saga.ProductId = x.Message.ProductId)
                    .Publish(context => new StockValidatingEvent { Code = context.Saga.CorrelationId, ProductId = context.Message.ProductId })
                    .Then(context => { Console.WriteLine($"Evento StockValidatingEvent publicado para {context.Saga.CorrelationId}\n "); })
                    .TransitionTo(OrderInitializedState));

            During(OrderInitializedState,
                When(StockValidatingEvent)
                    .Then(context => { Console.WriteLine($"2 - During OrderInitializedState When StockValidatingEvent para {context.Saga.CorrelationId} "); })
                    .Then(x => x.Saga.ProductId = x.Message.ProductId)
                    .Publish(context => new PaymentValidatingEvent { Code = context.Saga.CorrelationId, ProductId = context.Message.ProductId })
                    .Then(context => { Console.WriteLine($"Evento PaymentValidatingEvent publicado para {context.Saga.CorrelationId}\n "); })
                    .TransitionTo(StockValidatedState));

            During(StockValidatedState,
                When(PaymentValidatingEvent)
                    .Then(context => { Console.WriteLine($"3 - During StockValidatedState When PaymentValidatingEvent para {context.Saga.CorrelationId} "); })
                    .Then(x => x.Saga.ProductId = x.Message.ProductId)
                    .Publish(context => new OrderFinalizatingEvent { Code = context.Saga.CorrelationId, ProductId = context.Message.ProductId })
                    .Then(context => { Console.WriteLine($"Evento OrderFinalizatingEvent publicado para {context.Saga.CorrelationId}\n "); })
                    .TransitionTo(PaymentValidatedState));

            During(PaymentValidatedState,
                When(OrderFinalizatingEvent)
                    .Then(context => { Console.WriteLine($"4 - During PaymentValidatedState When OrderFinalizatingEvent para {context.Saga.CorrelationId} "); })
                    .Then(x => x.Saga.ProductId = x.Message.ProductId)
                    .Publish(context => new OrderFinishedEvent { Code = context.Saga.CorrelationId, ProductId = context.Message.ProductId })
                    .Then(context => { Console.WriteLine($"Evento OrderFinishedEvent publicado para {context.Saga.CorrelationId}\n "); })
                    .Finalize());

            During(Final,
                When(OrderFinishedEvent)
                    .Then(context => { Console.WriteLine($"5 - During Final When OrderFinishedEvent para {context.Saga.CorrelationId} "); })
                    .Then(x => x.Saga.ProductId = x.Message.ProductId)
                    .Finalize());


            //During(Initial,
            //    When(OrderInitializingEvent)
            //        .Then(x => x.Saga.OrderStartDate = DateTime.Now)
            //        .Then(x => x.Saga.ProductId = x.Message.ProductId)
            //        .Publish(context => new StockValidatingEvent { Code = context.Saga.CorrelationId, ProductId = context.Message.ProductId })
            //        .TransitionTo(OrderInitializedState));

            //During(OrderInitializedState,
            //    When(StockValidatingEvent)
            //        .Then(x => x.Saga.ProductId = x.Message.ProductId)
            //        .Publish(context => new PaymentValidatingEvent { Code = context.Saga.CorrelationId, ProductId = context.Message.ProductId })
            //        .TransitionTo(StockValidatedState));

            //During(StockValidatedState,
            //    When(PaymentValidatingEvent)
            //        .Then(x => x.Saga.ProductId = x.Message.ProductId)
            //        .Publish(context => new OrderFinalizatingEvent { Code = context.Saga.CorrelationId, ProductId = context.Message.ProductId })
            //        .TransitionTo(PaymentValidatedState));

            //During(PaymentValidatedState,
            //    When(OrderFinalizatingEvent)
            //        .Then(x => x.Saga.ProductId = x.Message.ProductId)
            //        .Send(context => new OrderFinishedEvent { Code = context.Saga.CorrelationId, ProductId = context.Message.ProductId })
            //        .TransitionTo(OrderFinishedState));



            DuringAny(When(OrderInitializingFaultEvent)
                .TransitionTo(OrderInitializedFaultedState)
                .Then(context => context.Publish<Fault<StockValidatingEvent>>(new { context.Message })));

            DuringAny(When(StockValidatingFaultEvent)
                .TransitionTo(StockValidatedFaultedState)
                .Then(context => context.Publish<Fault<PaymentValidatingEvent>>(new { context.Message })));

            DuringAny(When(PaymentValidatingFaultEvent)
                .TransitionTo(PaymentValidatedFaultedState)
                .Then(context => context.Publish<Fault<OrderFinalizatingEvent>>(new { context.Message })));

            DuringAny(When(OrderFinalizatingFaultEvent)
                .TransitionTo(OrderFinishedFaultedState)
                .Then(context => context.Publish<OrderProcessFailedEvent>(new { Code = context.Saga.CorrelationId })));

            DuringAny(When(OrderProcessFailedEvent)
                .TransitionTo(OrderFailedState));

        }
    }
}
