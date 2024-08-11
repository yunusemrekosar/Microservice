using EventBusBase.Events;

namespace NotificationService.IntegrationEvents.Event
{
    public class OrderPaymentSuccessIntegrationEvent : IntegrationEvent
    {
        public Guid OrderId { get; }

        public OrderPaymentSuccessIntegrationEvent(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
