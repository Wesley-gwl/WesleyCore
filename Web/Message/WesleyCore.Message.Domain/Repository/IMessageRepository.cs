using System;
using System.Collections.Generic;
using System.Text;
using WesleyCore.Infrastruction.Core;

namespace WesleyCore.Message.Domain.Repository
{
    /// <summary>
    /// 消息仓储
    /// </summary>
    public interface IMessageRepository : IRepository<Message, Guid>
    {
    }
}