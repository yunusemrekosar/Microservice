using EventBusBase.Abstraction;
using EventBusAzureServiceBus;
using EventBusRabbitMQ;
using EventBusBase;

namespace EventBusFactory
{
    public static class EventBusFactory
    {
        public static IEventBus Create(EventBusConfig config, IServiceProvider serviceProvider)
        {
            return config.EventBusType switch
            {
                EventBusType.AzureServiceBus => new AzureServiceBus(config, serviceProvider),
                _ => new RabbitMQService(config, serviceProvider),
            };
        }
    }
}
