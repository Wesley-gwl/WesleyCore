using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;
using WesleyCore.Domin.Abstractions;
using WesleyCore.User.Domain.Enums.User;

namespace WesleyCore.User.Domain
{
    /// <summary>
    /// 用户
    /// </summary>
    [Table("User", Schema = "System")]
    public class User : Entity<Guid>, IAggregateRoot, IMustHaveTenant, ISoftDelete
    {
        protected User()
        {
        }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string UserName { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        [Required]
        [Column(TypeName = "nvarchar(20)")]
        [IndexColumn]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 值类型属性
        /// </summary>
        public UserInfo UserInfo { get; set; }

        /// <summary>
        /// 租户
        /// </summary>
        [IndexColumn]
        public int TenantId { get; set; }

        /// <summary>
        /// 删除键
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}