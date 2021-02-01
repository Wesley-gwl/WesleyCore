using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;
using WesleyCore.Domin.Abstractions;
using WesleyCore.User.Domain.Events.User;

namespace WesleyCore.User.Domain
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
        /// 构造创建会员
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="company"></param>
        /// <param name="allowUserNumber"></param>
        public Member(string userName, string phoneNumber, string company, int allowUserNumber) : this()
        {
            var dtNow = DateTime.Now;
            UserName = userName;
            PhoneNumber = phoneNumber;
            Status = MemberStatusEnum.试用;
            CreateTime = dtNow;
            AllowUserNumber = allowUserNumber;
            MemberShip = new List<MemberShip>()
            {
                new MemberShip()
                {
                    CreateTime =dtNow,
                    StartTime = dtNow,
                    EndTime =dtNow.AddMonths(3),
                    Status= MemberShipStatusEnum.有效,
                    Memo = "试用"
                }
            };
            Company = new Company()
            {
                CompanyName = company,
            };
        }

        #region 字段

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
        /// 公司
        /// </summary>
        public Company Company { get; set; }

        /// <summary>
        /// 删除键
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public List<MemberShip> MemberShip { get; set; }

        #endregion 字段

        #region 方法

        /// <summary>
        /// 验证会员状态
        /// </summary>
        public void VerifyMemberShip()
        {
            if (Status == MemberStatusEnum.停用 || Status == MemberStatusEnum.默认)
            {
                throw new WlException("会员未激活，请联系管理员!");
            }
            if (MemberShip == null || MemberShip.Count == 0)
            {
                throw new WlException("会员未激活，请联系管理员!");
            }
            if (MemberShip.First().EndTime < DateTime.Now)
            {
                if (Status == MemberStatusEnum.试用 || Status == MemberStatusEnum.会员)
                {
                    UpdateStatus(MemberStatusEnum.默认);
                }
                throw new WlException("会员已过期，请联系管理员!");
            }
            if (MemberShip.First().StartTime > DateTime.Now)
            {
                throw new WlException($"会员未到开通时间,开通时间为{MemberShip.First().StartTime}，请联系管理员!");
            }
        }

        /// <summary>
        /// 新增用户领域
        /// </summary>
        public void CreateMemberUser(string password)
        {
            //新增user表 创建用户后 事件风暴 创建菜单FeatureMenu 发送消息等
            this.AddDomainEvent(new CreateUserDomainEvent(Id, PhoneNumber, password, UserName));
        }

        #endregion 方法

        #region 私有方法

        /// <summary>
        /// 更新会员状态
        /// </summary>
        /// <param name="默认"></param>
        /// <returns></returns>
        private void UpdateStatus(MemberStatusEnum status)
        {
            this.Status = status;
            this.AddDomainEvent(new UpdateMemberStatusDomainEvent(this));
        }

        #endregion 私有方法
    }
}