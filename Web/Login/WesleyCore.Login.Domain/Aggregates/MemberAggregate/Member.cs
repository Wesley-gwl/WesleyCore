using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;
using WesleyCore.Domin.Abstractions;

namespace WesleyCore.Login
{
    /// <summary>
    /// 会员表
    /// </summary>
    [Table("Member", Schema = "System")]
    public class Member : Entity<int>, ISoftDelete, IAggregateRoot
    {
        /// <summary>
        /// 构造
        /// </summary>
        protected Member()
        {
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="company"></param>
        /// <param name="status"></param>
        /// <param name="createTime"></param>
        /// <param name="allowUserNumber"></param>
        public Member(string userName, string phoneNumber, string company, MemberStatusEnum status, DateTime createTime, int allowUserNumber) : this()
        {
            UserName = userName;
            PhoneNumber = phoneNumber;
            Company = company;
            Status = status;
            CreateTime = createTime;
            AllowUserNumber = allowUserNumber;
            MemberShip = new List<MemberShip>();
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
        /// 过期时间
        /// </summary>
        public List<MemberShip> MemberShip { get; set; }
    }
}