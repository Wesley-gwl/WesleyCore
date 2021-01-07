using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using WesleyCore.Infrastruction.Core.Behaviors;

namespace WesleyCore.EntityFrameworkCore
{
    /// <summary>
    /// 事务处理
    /// </summary>
    public class MemberContextTransactionBehavior<TRequest, TResponse> : TransactionBehavior<MemberContext, TRequest, TResponse>
    {
        public MemberContextTransactionBehavior(MemberContext dbContext, ILogger<MemberContextTransactionBehavior<TRequest, TResponse>> logger) : base(dbContext, logger)
        {
        }
    }
}