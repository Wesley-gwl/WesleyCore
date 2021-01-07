using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WesleyCore.Domain.Const;
using WesleyCore.Domain.Enums.Member;
using WesleyCore.Domin.Abstractions;

namespace WesleyCore.Domain.Aggregate.MemberAggregate
{
    /// <summary>
    /// 会员时间表
    /// </summary>
    [Table("MemberShip", Schema = EfCoreConsts.SchemaNames.Member)]
    public class MemberShip : Entity<Guid>, ISoftDelete
    {
        /// <summary>
        /// 会员主键
        /// </summary>
        public int MemberId { get; set; }

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
    }
}