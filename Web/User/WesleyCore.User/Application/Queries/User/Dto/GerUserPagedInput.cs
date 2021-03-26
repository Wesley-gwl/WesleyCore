using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WesleyCore.Domin.Abstractions;
using WesleyCore.User.Application.Queries.Login.Dto;
using WesleyCore.User.Proto;

namespace WesleyCore.User.Application.Queries.User.Dto
{
    /// <summary>
    /// 分页
    /// </summary>
    public class GerUserPagedInput : PagedInputDto, IRequest<PagedReturn<UserDto>>
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public string SearchText { get; set; }
    }
}