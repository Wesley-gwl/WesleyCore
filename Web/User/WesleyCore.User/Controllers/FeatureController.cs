using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WesleyCore.Controllers;
using WesleyCore.Web;

namespace WesleyCore.Feature.Controllers
{
    /// <summary>
    /// 菜单功能
    /// </summary>
    public class FeatureController : WesleyCoreAPIBaseController
    {
        /// <summary>
        /// 中介
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// 构造
        /// </summary>
        public FeatureController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// 用户主菜单-并把PC功能权限放到缓存
        /// </summary>
        /// <param name="search">检索</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<BizResult<List<Tree>>> GetPCMenu(string search = null)
        {
            GetUser();
            var re = await _mediator.Send(new GetFeatureMenuTreeInput()
            {
                Search = search,
                UserId = Member.UserId,
                IsAdmin = Member.IsAdmin
            }, HttpContext.RequestAborted);
            return new BizResult<List<Tree>>(re);
        }
    }
}