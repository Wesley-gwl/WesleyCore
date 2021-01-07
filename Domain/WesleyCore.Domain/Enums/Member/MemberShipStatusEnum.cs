using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WesleyCore.Domain.Enums.Member
{
    /// <summary>
    /// 会员有效期状态
    /// </summary>
    public enum MemberShipStatusEnum
    {
        /// <summary>
        /// 有效
        /// </summary>
        [Description("有效")]
        有效,

        /// <summary>
        /// 停用(注销)
        /// </summary>
        [Description("无效")]
        无效,
    }
}