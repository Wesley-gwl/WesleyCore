using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WesleyCore.Domin.Abstractions;

namespace WesleyCore.Infrastruction.Core
{
    /// <summary>
    /// 领域事件
    /// </summary>
    public class EventBus : IEventBus
    {
        protected IMediator _mediator;

        public EventBus(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 发送领域事件
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public async Task PublishAsync(IDomainEvent domain)
        {
            await _mediator.Publish<INotification>(domain);
        }
    }
}