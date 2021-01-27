using Microsoft.Extensions.Logging;
using WesleyCore.Infrastruction.Core.Behaviors;
using WesleyCore.Infrastructure;

namespace WesleyCore.EntityFrameworkCore
{
    /// <summary>
    /// 事务处理
    /// </summary>
    public class CustomerContextTransactionBehavior<TRequest, TResponse> : TransactionBehavior<CustomerContext, TRequest, TResponse>
    {
        public CustomerContextTransactionBehavior(CustomerContext dbContext, ILogger<CustomerContextTransactionBehavior<TRequest, TResponse>> logger) : base(dbContext, logger)
        {
        }
    }
}