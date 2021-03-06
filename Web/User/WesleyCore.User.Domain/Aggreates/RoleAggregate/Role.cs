﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WesleyCore.Domin.Abstractions;

namespace WesleyCore.User.Domain
{
    /// <summary>
    /// 角色
    /// </summary>
    [Table("Role", Schema = "System")]
    public class Role : Entity<Guid>, ISoftDelete, IMustHaveTenant, IAggregateRoot
    {
        /// <summary>
        /// 构造
        /// </summary>
        protected Role()
        {
        }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(25)]
        public string Name { get; set; }

        /// <summary>
        /// 拼音码
        /// </summary>
        [StringLength(25)]
        public string NameLetter { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Memo { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantId { get; set; }
    }
}