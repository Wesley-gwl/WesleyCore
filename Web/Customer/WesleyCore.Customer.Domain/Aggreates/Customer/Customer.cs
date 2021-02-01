using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;
using WesleyCore.Domin.Abstractions;
using WesleyCore.Enums;

namespace WesleyCore.Customer.Domain
{
    /// <summary>
    /// 客户供应商来往单位
    /// </summary>
    [Table("Customer", Schema = "HR")]
    public class Customer : Entity<Guid>, IAggregateRoot, IMustHaveTenant, ISoftDelete
    {
        /// <summary>
        /// 构造
        /// </summary>
        protected Customer()
        {
        }

        /// <summary>
        /// 分类id
        /// </summary>
        public Guid CustomerTypeId { get; private set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        [StringLength(25)]
        public string CustomerTypeName { get; private set; }

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
        /// 拼音码快速检索使用
        /// </summary>
        [StringLength(25)]
        public string NameLetter { get; private set; }

        /// <summary>
        /// 电话,可以存多个
        /// </summary>
        [StringLength(100)]
        public string PhoneNumber { get; private set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        [StringLength(50)]
        public string CompanyName { get; private set; }

        /// <summary>
        /// 公司地址
        /// </summary>
        [StringLength(200)]
        public string Address { get; private set; }

        /// <summary>
        /// 物流地址
        /// </summary>
        [StringLength(200)]
        public string LogisticsAddress { get; private set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        [StringLength(6)]
        public string Postcode { get; private set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [StringLength(50)]
        public string Email { get; private set; }

        /// <summary>
        /// 传真
        /// </summary>
        [StringLength(20)]
        public string Faxes { get; private set; }

        /// <summary>
        /// 客户供应商等级
        /// </summary>
        public CustomerGradeEnum Grade { get; private set; }

        /// <summary>
        /// 客户供应商结算方式
        /// </summary>
        public CustomerPaymentTypeEnum PaymentType { get; private set; }

        /// <summary>
        /// 状态
        /// </summary>
        public CustomerStatusEnum Status { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Memo { get; private set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; private set; } = DateTime.Now;

        /// <summary>
        /// 租户
        /// </summary>
        [IndexColumn]
        public int TenantId { get; set; }

        /// <summary>
        /// 软删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}