using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WesleyCore.Message.Application.IntegrationEvents.Message
{
    /// <summary>
    /// 接口
    /// </summary>
    public interface IMessageSubscriberService
    {
        /// <summary>
        /// 订阅创建消息
        /// </summary>
        /// <param name="event"></param>
        Task CreateMessage(CreateMessageIntegrationEvent @event);
    }
}