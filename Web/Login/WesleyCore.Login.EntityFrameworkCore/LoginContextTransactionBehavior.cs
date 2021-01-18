using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using WesleyCore.Infrastruction.Core.Behaviors;
using WesleyCore.Login.Infrastructure;

namespace WesleyCore.EntityFrameworkCore
{
    /// <summary>
    /// 事务处理
    /// </summary>
    public class LoginContextTransactionBehavior<TRequest, TResponse> : TransactionBehavior<LoginContext, TRequest, TResponse>
    {
        public LoginContextTransactionBehavior(LoginContext dbContext, ILogger<LoginContextTransactionBehavior<TRequest, TResponse>> logger) : base(dbContext, logger)
        {
        }
    }
}