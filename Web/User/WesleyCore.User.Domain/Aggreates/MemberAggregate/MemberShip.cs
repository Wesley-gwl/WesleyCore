using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WesleyCore.Domin.Abstractions;

namespace WesleyCore.User
{
    /// <summary>
    /// 会员时间表
    /// </summary>
    [Table("MemberShip", Schema = "System")]
    public class MemberShip : ValueObject, ISoftDelete
    {
        /// <summary>
        /// 会员有效开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 会员有效结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public MemberShipStatusEnum Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Memo { get; set; }

        /// <summary>
        /// 删除键
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return StartTime;
            yield return EndTime;
            yield return Status;
            yield return CreateTime;
            yield return Memo;
        }
    }
}