using DotNetCore.CAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WesleyCore.Application.IntegrationEvents.Message;
using WesleyCore.Domin.Abstractions;
using WesleyCore.User.Domain.Events.User;

namespace WesleyCore.User.Application.DomainEventHendlers.User
{
    /// <summary>
    /// 发送创建消息
    /// </summary>
    public class SendUserMessageDomainEventHandler : IDomainEventHandler<SendUserMessageDomainEvent>
    {
        /// <summary>
        /// 推送
        /// </summary>
        private ICapPublisher _capPublisher;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="capPublisher"></param>
        public SendUserMessageDomainEventHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        /// <summary>
        /// 发送创建消息
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(SendUserMessageDomainEvent notification, CancellationToken cancellationToken)
        {
            await _capPublisher.PublishAsync("CreateMessage", new CreateMessageIntegrationEvent(notification.Title, notification.Content, notification.Type, notification.SenderID, notification.SenderName, notification.UserList));
        }
    }
}