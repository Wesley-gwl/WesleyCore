using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WesleyCore.Domin.Abstractions;

namespace WesleyCore.User.Domain
{
    /// <summary>
    /// 公司信息表
    /// </summary>
    [Table("Company", Schema = "System")]
    [Owned]
    public class Company : ValueObject
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        [StringLength(50)]
        public string CompanyName { get; set; }

        /// <summary>
        /// 电话座机号
        /// </summary>
        [StringLength(20)]
        public string MachineNumber { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [StringLength(50)]
        public string Linkman { get; set; }

        /// <summary>
        /// 公司地址
        /// </summary>
        [StringLength(200)]
        public string Address { get; set; }

        /// <summary>
        /// 银行卡号
        /// </summary>
        [StringLength(20)]
        public string BankNo { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        [StringLength(20)]
        public string Fax { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Memo { get; set; }

        /// <summary>
        /// 原子值
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return CompanyName;
            yield return MachineNumber;
            yield return Linkman;
            yield return Address;
            yield return BankNo;
            yield return Fax;
            yield return Memo;
        }
    }
}