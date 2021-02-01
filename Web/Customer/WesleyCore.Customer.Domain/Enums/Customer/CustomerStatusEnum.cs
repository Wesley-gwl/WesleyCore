using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WesleyCore.Enums
{
    /// <summary>
    /// 客户状态
    /// </summary>
    public enum CustomerStatusEnum
    {
        /// <summary>
        /// 全部
        /// </summary>
        [Description("全部")]
        全部 = -1,

        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        启用 = 0,

        /// <summary>
        /// 停用
        /// </summary>
        [Description("停用")]
        停用,
    }
}