using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WesleyCore.Application.Commands;
using WesleyCore.Ordering.API.Application.Queries;

namespace WesleyCore.Web.Controllers.ApiControllers
{
    /// <summary>
    /// 订单
    /// </summary>
    public class OrderController : WesleyCoreAPIBaseController
    {
        private IMediator _mediator;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="mediator"></param>
        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<long> CreateOrder(CreateOrderCommand cmd)
        {
            return await _mediator.Send(cmd, HttpContext.RequestAborted);
        }

        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="myOrderQuery"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<string>> QueryOrder([FromQuery] OrderQuery myOrderQuery)
        {
            return await _mediator.Send(myOrderQuery);
        }
    }
}