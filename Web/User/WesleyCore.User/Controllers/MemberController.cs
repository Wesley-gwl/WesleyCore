using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WesleyCore.Controllers;
using WesleyCore.User.Application.Commands.Member;
using WesleyCore.Web;
using WesleyUntity;

namespace WesleyCore.User.Controllers
{
    /// <summary>
    /// 会员
    /// </summary>
    public class MemberController : WesleyCoreAPIBaseController
    {
        /// <summary>
        /// 中介
        /// </summary>
        private readonly IMediator _mediator;

        private readonly IConfiguration configuration;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="configuration"></param>
        public MemberController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            this.configuration = configuration;
        }

        /// <summary>
        ///  注册
        /// </summary>
        /// <param name="dto">注册dto</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<BizResult<string>> Register(CreateMemberCommand dto)
        {
            var hash = configuration["Customization:PwdKey"];
            dto.Password = EncryptUtil.AESEncrypt(dto.Password, hash);
            await _mediator.Send(dto, HttpContext.RequestAborted);
            return new BizResult<string>("注册成功");
        }
    }
}