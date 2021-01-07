using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WesleyCore.Domain.Enums.Member
{
    /// <summary>
    /// 会员状态
    /// </summary>
    public enum MemberStatusEnum
    {
        /// <summary>
        /// 全部
        /// </summary>
        [Description("全部")]
        全部 = -1,

        /// <summary>
        /// 默认(申请中状态)
        /// </summary>
        [Description("默认")]
        默认 = 0,

        /// <summary>
        /// 试用(初次激活)
        /// </summary>
        [Description("试用")]
        试用,

        /// <summary>
        /// 会员(上帝)
        /// </summary>
        [Description("会员")]
        会员,

        /// <summary>
        /// 停用(注销)
        /// </summary>
        [Description("停用")]
        停用,
    }
}