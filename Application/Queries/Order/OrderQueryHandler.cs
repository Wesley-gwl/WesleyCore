using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GeekTime.Ordering.API.Application.Queries
{
    /// <summary>
    /// 查询订单
    /// </summary>
    public class OrderQueryHandler : IRequestHandler<OrderQuery, List<string>>
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<List<string>> Handle(OrderQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new List<string>() { DateTime.Now.ToString() });
        }
    }
}