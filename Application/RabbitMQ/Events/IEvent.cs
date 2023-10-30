namespace Application.RabbitMQ.Events;

internal interface IEvent
{
    Guid Id { get; }
}
