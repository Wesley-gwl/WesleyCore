using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WesleyCore.Domin.Abstractions;

namespace WesleyCore.User.Domain.Aggreates.UserAggregate
{
    /// <summary>
    /// 用户角色关联表
    /// </summary>
    [Table("UserRole", Schema = "System")]
    public class UserRole : Entity<long>
    {
        /// <summary>
        /// 构造
        /// </summary>
        protected UserRole()
        {
        }

        /// <summary>
        /// 用户主键
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Role主键
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}