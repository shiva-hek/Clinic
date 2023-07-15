namespace Shared.Domain;

public interface IDomainEvent
{
    DateTime DateTimeEventOccurred { get; }
}