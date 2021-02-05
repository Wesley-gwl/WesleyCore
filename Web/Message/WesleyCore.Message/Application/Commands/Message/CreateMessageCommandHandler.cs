using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WesleyCore.Message.Domain.Repository;

namespace WesleyCore.Message.Application.Commands.Message
{
    /// <summary>
    /// 创建消息
    /// </summary>
    public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, bool>
    {
        private readonly IMessageRepository _messageRepository;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="messageRepository"></param>
        public CreateMessageCommandHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        /// <summary>
        /// 新增消息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            await _messageRepository.AddAsync(new Domain.Message(request.Title, request.Content, request.Type, request.SenderID, request.SenderName, request.UserList));
            return true;
        }
    }
}