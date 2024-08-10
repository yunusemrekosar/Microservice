using EventBusBase.Abstraction;
using EventBusBase;
using PaymentService.Api.IntegrationEvents.EventHandler;
using RabbitMQ.Client;
using PaymentService.Api.IntegrationEvents.Event;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogging(configure =>
{
    configure.AddConsole();
    configure.AddDebug();
});
builder.Services.AddTransient<OrderStartedIntegrationEventHandler>();

builder.Services.AddSingleton<IEventBus>(sp =>
{
    EventBusConfig config = new()
    {
        ConnectionRetryCount = 5,
        EventNameSuffix = "IntegrationEvent",
        SubscriberClientAppName = "PaymentService",
        EventBusType = EventBusType.RabbitMQ,
    };
    
   return EventBusFactory.EventBusFactory.Create(config, sp);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


IEventBus eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<OrderStartedIntegrationEvent, OrderStartedIntegrationEventHandler>();

app.Run();
