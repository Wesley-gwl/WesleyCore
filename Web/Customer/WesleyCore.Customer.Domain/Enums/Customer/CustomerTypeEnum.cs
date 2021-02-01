using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WesleyCore.Enums
{
    /// <summary>
    /// 客户供应商分类
    /// </summary>
    public enum CustomerTypeEnum
    {
        /// <summary>
        /// 全部
        /// </summary>
        [Description("全部")]
        全部 = -1,

        /// <summary>
        /// 客户
        /// </summary>
        [Description("客户")]
        客户 = 0,

        /// <summary>
        /// 供应商
        /// </summary>
        [Description("供应商")]
        供应商,

        /// <summary>
        /// 往来商户(同市场商家存在调货的客户供应商)
        /// </summary>
        [Description("往来商户")]
        往来商户
    }
}