using MediatR;

namespace WesleyCore.Domin.Abstractions
{
    /// <summary>
    /// 领域事件
    /// </summary>
    /// <typeparam name="TDomainEvent"></typeparam>
    public interface IDomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent>
      where TDomainEvent : IDomainEvent
    {
    }
}