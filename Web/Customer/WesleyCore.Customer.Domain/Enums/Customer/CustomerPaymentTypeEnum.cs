using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WesleyCore.Enums
{
    /// <summary>
    /// 客户结算方式
    /// </summary>
    public enum CustomerPaymentTypeEnum
    {
        /// <summary>
        /// 全部
        /// </summary>
        [Description("全部")]
        全部 = -1,

        /// <summary>
        /// 日结
        /// </summary>
        [Description("日结")]
        日结 = 0,

        /// <summary>
        /// 月结
        /// </summary>
        [Description("当月结")]
        当月结,

        /// <summary>
        /// 月结
        /// </summary>
        [Description("月结")]
        月结,

        /// <summary>
        /// 季结
        /// </summary>
        [Description("季结")]
        季结,

        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他结算")]
        其他结算
    }
}