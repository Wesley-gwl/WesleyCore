using System;
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
        protected User()
        {
        }

        /// <summary>
        ///
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
            UserInfo = new UserInfo();
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
        /// 值类型属性
        /// </summary>
        public UserInfo UserInfo { get; private set; }

        /// <summary>
        /// 租户
        /// </summary>
        [IndexColumn]
        public int TenantId { get; set; }

        /// <summary>
        /// 删除键
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 验证登录
        /// </summary>
        /// <param name="password"></param>
        public void VerifyLogin(string password)
        {
            if (UserInfo.Status == UserStatusEnum.离职)
            {
                throw new WlException($"用户已离职");
            }
            if (Password != password)
            {
                //该账号1小时内的登录错误次数
                var errCount = RedisClient.RedisCt.GetStringKey(RedisConst.AccessFailed + PhoneNumber).ToInt(0).Value;
                //记录错误登录次数
                RedisClient.RedisCt.SetStringKey(RedisConst.AccessFailed + PhoneNumber, ++errCount);
                throw new WlException($"密码错误{errCount}次");
            }
            //正确登录后清掉计数
            RedisClient.RedisCt.KeyDelete(RedisConst.AccessFailed + PhoneNumber);
        }
    }
}