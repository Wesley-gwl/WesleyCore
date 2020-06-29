using DotNetCore.CAP;
using Microsoft.Extensions.Logging;
using WesleyCore.Infrastruction.Core.Behaviors;

namespace WesleyCore.Infrastruction
{
    /// <summary>
    /// 事务处理
    /// </summary>
    public class DomainContextTransactionBehavior<TRequest, TResponse> : TransactionBehavior<DomainContext, TRequest, TResponse>
    {
        public DomainContextTransactionBehavior(DomainContext dbContext, ILogger<DomainContextTransactionBehavior<TRequest, TResponse>> logger) : base(dbContext, logger)
        {
        }
    }
}