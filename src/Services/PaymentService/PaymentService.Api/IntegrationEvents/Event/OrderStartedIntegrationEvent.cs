using EventBusBase.Events;

namespace PaymentService.Api.IntegrationEvents.Event
{
    public class OrderStartedIntegrationEvent : IntegrationEvent
    {
        public Guid OrderId { get; set; }

        public OrderStartedIntegrationEvent()
        {

        }

        public OrderStartedIntegrationEvent(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
