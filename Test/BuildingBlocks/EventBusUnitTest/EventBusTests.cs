using EventBusBase;
using EventBusBase.Abstraction;
using EventBusUnitTest.Events.EventHandlers;
using EventBusUnitTest.Events.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace EventBusUnitTest
{
    [TestClass]
    public class EventBusTests
    {
        private ServiceCollection _services;

        public EventBusTests()
        {
            _services = new ServiceCollection();
            _services.AddLogging(configure => configure.AddConsole());
        }

        [TestMethod]
        public void subscribe_event_on_rabbitmq_test()
        {
            _services.AddSingleton<IEventBus>(sp =>
            {
                return EventBusFactory.EventBusFactory.Create(GetRabbitMQConfig(), sp);
            });


            var sp = _services.BuildServiceProvider();

            var eventBus = sp.GetRequiredService<IEventBus>();

            eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
            //eventBus.UnSubscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
        }

        [TestMethod]
        public void send_message_to_rabbitmq_test()
        {
            _services.AddSingleton<IEventBus>(sp =>
            {
                return EventBusFactory.EventBusFactory.Create(GetRabbitMQConfig(), sp);
            });


            var sp = _services.BuildServiceProvider();

            var eventBus = sp.GetRequiredService<IEventBus>();

            eventBus.Publish(new OrderCreatedIntegrationEvent(2));

        }

        //[TestMethod]
        //public void subscribe_event_on_azure_test()
        //{
        //    _services.AddSingleton<IEventBus>(sp =>
        //    {
        //        return EventBusFactory.EventBusFactory.Create(GetAzureConfig(), sp);
        //    });


        //    var sp = _services.BuildServiceProvider();

        //    var eventBus = sp.GetRequiredService<IEventBus>();

        //    eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
        //    eventBus.UnSubscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();

        //    Task.Delay(2000).Wait();
        //}

        //[TestMethod]
        //public void send_message_to_azure_test()
        //{
        //    _services.AddSingleton<IEventBus>(sp =>
        //    {
        //        return EventBusFactory.EventBusFactory.Create(GetAzureConfig(), sp);
        //    });


        //    var sp = _services.BuildServiceProvider();

        //    var eventBus = sp.GetRequiredService<IEventBus>();

        //    eventBus.Publish(new OrderCreatedIntegrationEvent(1));
        //}





        private EventBusConfig GetAzureConfig()
        {
            return new EventBusConfig()
            {
                ConnectionRetryCount = 5,
                SubscriberClientAppName = "EventBus.UnitTest",
                DefaultTopicName = "TopicName",
                EventBusType = EventBusType.AzureServiceBus,
                EventNameSuffix = "IntegrationEvent",
                //EventBusConnectionString = "Endpoint=sb://techbuddy.servicebus.windows.net/;SharedAccessKeyName=NewPolicyForYTVideos;SharedAccessKey=7sJghGWFOXaUaRblrbzOIIf4bQk6qkbTN/SEnKjXLpE="
            };
        }

        private EventBusConfig GetRabbitMQConfig()
        {
            return new EventBusConfig()
            {
                ConnectionRetryCount = 5,
                SubscriberClientAppName = "EventBus.UnitTest",
                DefaultTopicName = "TopicName",
                EventBusType = EventBusType.RabbitMQ,
                EventNameSuffix = "IntegrationEvent",
                //Connection = new ConnectionFactory()
                //{ 
                //    HostName = "localhost",
                //    Port = 15672,
                //    UserName = "guest",
                //    Password = "guest"
                //}
            };
        }
    }
}