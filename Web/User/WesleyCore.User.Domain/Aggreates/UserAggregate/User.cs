using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;
using WesleyCore.Domin.Abstractions;
using WesleyCore.User.Domain.Const;
using WesleyCore.User.Domain.Enums.User;
using WesleyRedis;
using WesleyUntity;

namespace WesleyCore.User.Domain
{
    /// <summary>
    /// 用户
    /// </summary>
    [Table("User", Schema = "System")]
    public class User : Entity<Guid>, IAggregateRoot, IMustHaveTenant, ISoftDelete
    {
        /// <summary>
        /// 构造
        /// </summary>
        protected User()
        {
        }

        /// <summary>
        /// 新增赋值
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="password"></param>
        /// <param name="tenantId"></param>
        public User(string userName, string phoneNumber, string password, int tenantId)
        {
            UserName = userName;
            PhoneNumber = phoneNumber;
            Password = password;
            CreateTime = DateTime.Now;
            TenantId = tenantId;
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
        [StringLength(20)]
        [IndexColumn]
        public string PhoneNumber { get; private set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; private set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; private set; }

        /// <summary>
        /// 是否是管理员
        /// </summary>
        public bool IsAdmin { get; private set; }

        /// <summary>
        /// 状态
        /// </summary>
        public UserStatusEnum Status { get; private set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [StringLength(20)]
        public string IDCard { get; private set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        [StringLength(200)]
        public string ImageUrl { get; private set; }

        /// <summary>
        /// 地址
        /// </summary>
        [StringLength(200)]
        public string Address { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Memo { get; private set; }

        /// <summary>
        /// 租户
        /// </summary>
        [IndexColumn]
        public int TenantId { get; set; }

        /// <summary>
        /// 删除键
        /// </summary>
        public bool IsDeleted { get; set; }

        #endregion 字段

        #region 方法

        /// <summary>
        /// 验证登录
        /// </summary>
        /// <param name="password"></param>
        public void VerifyLogin(string password)
        {
            if (Status == UserStatusEnum.离职)
            {
                throw new WlException($"用户已离职");
            }
            if (Password != password)
            {
                //该账号1小时内的登录错误次数
                var errCount = RedisClient.RedisCt.GetStringKey(RedisConst.AccessFailed + PhoneNumber).ToInt(0).Value;
                //记录错误登录次数
                RedisClient.RedisCt.SetStringKey(RedisConst.AccessFailed + PhoneNumber, ++errCount, TimeSpan.FromMinutes(10));
                throw new WlException($"密码错误{errCount}次");
            }
            //正确登录后清掉计数
            RedisClient.RedisCt.KeyDelete(RedisConst.AccessFailed + PhoneNumber);
        }

        #endregion 方法
    }
}