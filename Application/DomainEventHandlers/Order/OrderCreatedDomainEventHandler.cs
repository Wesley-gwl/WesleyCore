using DotNetCore.CAP;
using GeekTime.Ordering.API.Application.IntegrationEvents;
using System.Threading;
using System.Threading.Tasks;
using WesleyCore.Domain.Events;
using WesleyCore.Domin.Abstractions;

namespace GeekTime.Ordering.API.Application.DomainEventHandlers
{
    /// <summary>
    /// 订单创建领域事件
    /// </summary>
    public class OrderCreatedDomainEventHandler : IDomainEventHandler<OrderCreateDomainEvent>
    {
        private readonly ICapPublisher _capPublisher;

        public OrderCreatedDomainEventHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        /// <summary>
        /// 领域事件接受事件
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(OrderCreateDomainEvent notification, CancellationToken cancellationToken)
        {
            //试下订单创建订阅事件
            await _capPublisher.PublishAsync("OrderCreated", new OrderCreatedIntegrationEvent(notification.Order.Id));
        }
    }
}