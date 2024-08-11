using Microsoft.Extensions.DependencyInjection;
using EventBusBase.Abstraction;
using EventBusBase;
using Microsoft.Extensions.Logging;
using NotificationService.IntegrationEvents.Event;
using NotificationService.IntegrationEvents.EventHandler;

internal class Program
{
    private static void Main(string[] args)
    {
        ServiceCollection services = new();
        ConfigureServices(services);

        IServiceProvider serviceProvider = services.BuildServiceProvider();
        IEventBus eventBus = serviceProvider.GetRequiredService<IEventBus>();

        eventBus.Subscribe<OrderPaymentFailedIntegrationEvent, OrderPaymentFailedIntegrationEventHandler>();
        eventBus.Subscribe<OrderPaymentSuccessIntegrationEvent, OrderPaymentSuccessIntegrationEventHandler>();


        Console.WriteLine("NotificationService is running...");
        Console.ReadKey();
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        services.AddLogging(configure =>
        {
            configure.AddConsole();
        });

         services.AddTransient<OrderPaymentFailedIntegrationEventHandler>();
        services.AddTransient<OrderPaymentSuccessIntegrationEventHandler>();

        services.AddSingleton<IEventBus>(sp =>
        {
            EventBusConfig config = new()
            {
                ConnectionRetryCount = 5,
                EventNameSuffix = "IntegrationEvent",
                SubscriberClientAppName = "NotificationService",
                EventBusType = EventBusType.RabbitMQ,
            };

            return EventBusFactory.EventBusFactory.Create(config, sp);
        });
    }
}