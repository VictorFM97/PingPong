namespace Application.RabbitMQ.Events;

public record GameFinishedEvent(
    string Winner) : IEvent
{
    public Guid Id { get; private set; } = Guid.NewGuid();
}
