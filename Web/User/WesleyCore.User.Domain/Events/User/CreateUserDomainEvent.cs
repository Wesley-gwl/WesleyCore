using WesleyCore.Domin.Abstractions;

namespace WesleyCore.User.Domain.Events.User
{
    /// <summary>
    /// 创建用户
    /// </summary>
    public class CreateUserDomainEvent : IDomainEvent
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="password"></param>
        /// <param name="userName"></param>
        /// <param name="isCreateMemu"></param>
        public CreateUserDomainEvent(int tenantId, string phoneNumber, string password, string userName, bool isCreateMemu = false)
        {
            TenantId = tenantId;
            PhoneNumber = phoneNumber;
            Password = password;
            UserName = userName;
            IsCreateMemu = isCreateMemu;
        }

        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantId { get; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoneNumber { get; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// 是否生成菜单
        /// </summary>
        public bool IsCreateMemu { get; }
    }
}