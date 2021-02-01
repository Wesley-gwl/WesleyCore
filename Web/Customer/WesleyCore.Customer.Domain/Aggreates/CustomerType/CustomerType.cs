using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;
using WesleyCore.Customer.Domain.Events;
using WesleyCore.Domin.Abstractions;
using WesleyCore.Enums;
using WesleyUntity;

namespace WesleyCore.Customer.Domain
{
    /// <summary>
    /// 客户供应商来往单位
    /// </summary>
    [Table("CustomerType", Schema = "HR")]
    public class CustomerType : Entity<Guid>, IAggregateRoot, IMustHaveTenant, ISoftDelete
    {
        /// <summary>
        /// 构造
        /// </summary>
        protected CustomerType()
        {
        }

        /// <summary>
        /// 构造新增分类
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="memo"></param>
        public CustomerType(CustomerTypeEnum type, string name, string memo)
        {
            Id = ComFunc.NewCombGuid();
            Type = type;
            Name = name;
            Memo = memo;
        }

        /// <summary>
        /// 分类
        /// </summary>
        public CustomerTypeEnum Type { get; private set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(25)]
        public string Name { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Memo { get; private set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

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