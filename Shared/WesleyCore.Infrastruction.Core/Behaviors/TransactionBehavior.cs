using DotNetCore.CAP;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace WesleyCore.Infrastruction.Core.Behaviors
{
    public class TransactionBehavior<TDbContext, TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private ILogger _logger;
        private TDbContext _dbContext;
        private ICapPublisher _capBus;

        public TransactionBehavior(TDbContext request, CancellationToken cancellationToken, RequestHandler<TRequest, TResponse> requestHandler)
        {
        }
    }
}