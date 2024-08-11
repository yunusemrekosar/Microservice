using NotificationService.IntegrationEvents.Event;
using EventBusBase.Abstraction;
using Microsoft.Extensions.Logging;

namespace NotificationService.IntegrationEvents.EventHandler
{
    public class OrderPaymentSuccessIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentSuccessIntegrationEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<OrderPaymentSuccessIntegrationEventHandler> _logger;

        public OrderPaymentSuccessIntegrationEventHandler(ILogger<OrderPaymentSuccessIntegrationEventHandler> logger, IEventBus eventBus)
        {
            _logger = logger;
            _eventBus = eventBus;
        }

        public Task Handle(OrderPaymentSuccessIntegrationEvent @event)
        {
            _logger.LogInformation($"OrderPaymentSucceded orderId: {@event.OrderId}");

            return Task.CompletedTask;
        }
    }
}
