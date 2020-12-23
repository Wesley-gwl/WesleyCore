using DotNetCore.CAP;
using MediatR;
using System.Threading.Tasks;
using WesleyCore.Infrastruction.Repositories;

namespace GeekTime.Ordering.API.Application.IntegrationEvents
{
    /// <summary>
    /// 订阅服务-接受RM的集成事件请求
    /// </summary>
    public class OrderSubscriberService : IOrderSubscriberService, ICapSubscribe
    {
        private IMediator _mediator;
        private IOrderRepository _orderRepository;

        public OrderSubscriberService(IMediator mediator, IOrderRepository orderRepository)
        {
            _mediator = mediator;
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// 创建订阅OrderCreated事件
        /// </summary>
        /// <param name="event"></param>
        [CapSubscribe("OrderCreated")]
        public Task OrderCreated(OrderCreatedIntegrationEvent @event)
        {
            return Task.CompletedTask;
            //
            //await _orderRepository.AddAsync(@event.OrderId);
            //Do SomeThing
        }
    }
}