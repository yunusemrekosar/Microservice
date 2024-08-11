using NotificationService.IntegrationEvents.Event;
using EventBusBase.Abstraction;
using Microsoft.Extensions.Logging;

namespace NotificationService.IntegrationEvents.EventHandler
{
    public class OrderPaymentFailedIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentFailedIntegrationEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<OrderPaymentFailedIntegrationEventHandler> _logger;

        public OrderPaymentFailedIntegrationEventHandler(ILogger<OrderPaymentFailedIntegrationEventHandler> logger, IEventBus eventBus)
        {
            _logger = logger;
            _eventBus = eventBus;
        }

        public Task Handle(OrderPaymentFailedIntegrationEvent @event)
        {
            _logger.LogInformation($"OrderPaymentFailed orderId: {@event.OrderId}, errorMessage: {@event.ErrorMessage}");

            return Task.CompletedTask;
        }
    }
}
