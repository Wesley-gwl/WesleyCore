using WesleyCore.Domain.OrderAggregate;
using WesleyCore.Domin.Abstractions;

namespace WesleyCore.Domain.Events
{
    /// <summary>
    /// 领域事件
    /// </summary>
    public class OrderCreateDomainEvent : IDomainEvent
    {
        public Order Order { get; private set; }

        public OrderCreateDomainEvent(Order order)
        {
            this.Order = order;
        }
    }
}