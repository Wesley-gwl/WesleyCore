using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WesleyCore.Dtos;

namespace WesleyCore
{
    /// <summary>
    /// 获取用户菜单
    /// </summary>
    public class GetFeatureMenuTreeInput : IRequest<List<Tree>>
    {
        /// <summary>
        /// 查询菜单名称
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        /// 当前用id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 是否是管理员
        /// </summary>
        public bool IsAdmin { get; set; }
    }
}