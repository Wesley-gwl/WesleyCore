using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WesleyCore.Controllers;
using WesleyCore.Web;

namespace WesleyCore.User.Controllers
{
    /// <summary>
    /// 用户api
    /// </summary>
    public class UserController : WesleyCoreAPIBaseController
    {
        /// <summary>
        /// 中介
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="mediator"></param>
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}