using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;
using WesleyCore.Domain.Const;
using WesleyCore.Domain.Enums.Member;
using WesleyCore.Domin.Abstractions;

namespace WesleyCore.Domain.Aggregate.MemberAggregate
{
    /// <summary>
    /// 会员表
    /// </summary>
    [Table("Member", Schema = EfCoreConsts.SchemaNames.System)]
    public class Member : Entity<int>, ISoftDelete, IAggregateRoot
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="company"></param>
        /// <param name="status"></param>
        public Member(string userName, string phoneNumber, string company, MemberStatusEnum status, DateTime createTime, int allowUserNumber)
        {
            UserName = userName;
            PhoneNumber = phoneNumber;
            Company = company;
            Status = status;
            CreateTime = createTime;
            AllowUserNumber = allowUserNumber;
        }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [StringLength(20)]
        public string UserName { get; private set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        [Required]
        [IndexColumn(IsUnique = true)]
        [StringLength(20)]
        public string PhoneNumber { get; private set; }

        /// <summary>
        /// 公司
        /// </summary>
        [StringLength(50)]
        public string Company { get; private set; }

        /// <summary>
        /// 会员状态
        /// </summary>
        public MemberStatusEnum Status { get; private set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; private set; }

        /// <summary>
        /// 运行员工数量（包括自己）
        /// </summary>
        public int AllowUserNumber { get; private set; }

        /// <summary>
        /// 删除键
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 过期时间内容
        /// </summary>
        [NotMapped]
        public MemberShip MemberShip { get; private set; }

        /// <summary>
        /// 创建过期时间
        /// </summary>
        public void InitializeMemberShip(DateTime startTime, DateTime endTime, string memo)
        {
            MemberShip.MemberId = Id;
            MemberShip.Memo = memo;
            MemberShip.StartTime = startTime;
            MemberShip.EndTime = endTime;
            MemberShip.CreateTime = DateTime.Now;
            MemberShip.Status = MemberShipStatusEnum.有效;
        }

        /// <summary>
        /// 获取最新的过期时间
        /// </summary>
        public void GetMemberShip()
        {
        }
    }
}