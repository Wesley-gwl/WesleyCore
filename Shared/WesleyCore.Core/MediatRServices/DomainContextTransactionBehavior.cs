using Microsoft.Extensions.Logging;
using WesleyCore.EntityFrameworkCore;
using WesleyCore.Infrastruction.Core.Behaviors;

namespace WesleyCore.Core.MediatRServices
{
    public class DomainContextTransactionBehavior<TRequest, TResponse> : TransactionBehavior<DomainContext, TRequest, TResponse>
    {
        public DomainContextTransactionBehavior(DomainContext dbContext, ILogger<DomainContextTransactionBehavior<TRequest, TResponse>> logger) : base(dbContext, logger)
        {
        }
    }
}