using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using WesleyCore.Infrastruction.Core.Behaviors;
using WesleyCore.Infrastructure;

namespace WesleyCore.EntityFrameworkCore
{
    /// <summary>
    /// 事务处理
    /// </summary>
    public class UserContextTransactionBehavior<TRequest, TResponse> : TransactionBehavior<UserContext, TRequest, TResponse>
    {
        public UserContextTransactionBehavior(UserContext dbContext, ILogger<UserContextTransactionBehavior<TRequest, TResponse>> logger) : base(dbContext, logger)
        {
        }
    }
}