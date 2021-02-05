using Microsoft.Extensions.Logging;
using WesleyCore.Infrastruction.Core.Behaviors;
using WesleyCore.Infrastructure;

namespace WesleyCore.EntityFrameworkCore
{
    /// <summary>
    /// 事务处理
    /// </summary>
    public class MessageContextTransactionBehavior<TRequest, TResponse> : TransactionBehavior<MessageContext, TRequest, TResponse>
    {
        public MessageContextTransactionBehavior(MessageContext dbContext, ILogger<MessageContextTransactionBehavior<TRequest, TResponse>> logger) : base(dbContext, logger)
        {
        }
    }
}