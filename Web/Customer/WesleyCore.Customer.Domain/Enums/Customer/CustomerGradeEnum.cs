using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WesleyCore.Enums
{
    /// <summary>
    /// 客户供应商等级
    /// </summary>
    public enum CustomerGradeEnum
    {
        /// <summary>
        /// 全部
        /// </summary>
        [Description("全部")]
        全部 = -4,

        /// <summary>
        /// 非常差
        /// </summary>
        [Description("非常差")]
        非常差 = -3,

        /// <summary>
        /// 较差
        /// </summary>
        [Description("差")]
        差 = -2,

        /// <summary>
        /// 较差
        /// </summary>
        [Description("较差")]
        较差 = -1,

        /// <summary>
        /// 默认值
        /// </summary>
        [Description("一般")]
        一般 = 0,

        /// <summary>
        /// 默认值
        /// </summary>
        [Description("较好")]
        较好 = 1,

        /// <summary>
        /// 默认值
        /// </summary>
        [Description("好")]
        好 = 2,

        /// <summary>
        /// 用户头像
        /// </summary>
        [Description("非常好")]
        VIP = 3
    }
}