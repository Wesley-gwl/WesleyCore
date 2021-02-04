using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WesleyCore.Controllers;
using WesleyCore.Customer.Application;
using WesleyCore.Customer.Application.Commands.CustomerType;
using WesleyCore.Customer.Application.Commands.CustomerType.Dto;
using WesleyCore.Web;

namespace WesleyCore.Customer.Controllers
{
    /// <summary>
    /// 客户供应商来往单位分类
    /// </summary>
    public class CustomerTypeController : WesleyCoreAPIBaseController
    {
        /// <summary>
        /// 中介
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="mediator"></param>
        public CustomerTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 保存分类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<BizResult> SaveCustomerType(CreateOrEditCustomerTypeCommand input)
        {
            await _mediator.Send(input, HttpContext.RequestAborted);
            return new BizResult("保存成功");
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<BizResult> DeleteCustomerType(Guid input)
        {
            await _mediator.Send(new DeleteCustomerTypeCommand(input), HttpContext.RequestAborted);
            return new BizResult("删除成功");
        }
    }
}