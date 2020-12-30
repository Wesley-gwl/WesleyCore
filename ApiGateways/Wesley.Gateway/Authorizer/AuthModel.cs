using System;

namespace Wesley.Web.Controllers
{
    /// <summary>
    /// 验证类
    /// </summary>
    public class AuthModel
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 是否是管理员
        /// </summary>
        public bool IsAdmin { get; set; } = false;

        /// <summary>
        /// 电话
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        public int TenantID { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime Exp { get; set; }
    }
}