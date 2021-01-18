using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WesleyCore.User.Domain.Enums.User
{
    /// <summary>
    /// 用户状态
    /// </summary>
    public enum UserStatusEnum
    {
        /// <summary>
        /// 在职
        /// </summary>
        [Description("在职")]
        在职,

        /// <summary>
        /// 离职
        /// </summary>
        [Description("离职")]
        离职,
    }
}