using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WesleyCore.Controllers;

namespace WesleyCore.Customer.Controllers
{
    /// <summary>
    /// 客户供应商来往单位接口
    /// </summary>
    public class CustomerController : WesleyCoreAPIBaseController
    {
        /// <summary>
        /// 中介
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="mediator"></param>
        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}