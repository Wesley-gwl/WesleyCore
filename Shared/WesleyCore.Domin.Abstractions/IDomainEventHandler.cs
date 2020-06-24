using MediatR;

namespace WesleyCore.Domin.Abstractions
{
    public interface IDomainEventHandler<IDomainEvent> : INotificationHandler<IDomainEvent> :where
    {
    }
}