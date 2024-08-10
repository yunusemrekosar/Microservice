using EventBusBase.Events;

namespace PaymentService.Api.IntegrationEvents.Event
{
    public class OrderPaymentSuccessIntegrationEvent : IntegrationEvent
    {
        public Guid OrderId { get; }

        public OrderPaymentSuccessIntegrationEvent(Guid orderId) => OrderId = orderId;
    }
}
