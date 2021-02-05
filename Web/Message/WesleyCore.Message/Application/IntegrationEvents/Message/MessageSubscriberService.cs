using DotNetCore.CAP;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WesleyCore.Message.Application.Commands.Message;
using WesleyCore.Message.Domain.Repository;

namespace WesleyCore.Message.Application.IntegrationEvents.Message
{
    /// <summary>
    /// 消息订阅方法
    /// </summary>
    public class MessageSubscriberService : IMessageSubscriberService, ICapSubscribe
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="mediator"></param>
        public MessageSubscriberService(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 创建订阅CreateMessage事件
        /// </summary>
        /// <param name="input"></param>
        [CapSubscribe("CreateMessage")]
        public async Task CreateMessage(CreateMessageIntegrationEvent input)
        {
            await _mediator.Send(new CreateMessageCommand(input.Title, input.Content, input.Type, input.SenderID, input.SenderName, input.UserList));
        }
    }
}