using System;
using WesleyCore.Infrastruction.Core;
using WesleyCore.Infrastructure;
using WesleyCore.User.Domain;

namespace WesleyCore.User.Infrastructure.Repositories
{
    /// <summary>
    /// 用户仓储
    /// </summary>
    public class UserRepository : Repository<Domain.User, Guid, UserContext>, IUserRepository
    {
        /// <summary>
        /// 链接
        /// </summary>
        private readonly UserContext _context;

        /// <summary>
        /// 租户获取
        /// </summary>
        private readonly ITenantProvider _tenantProvider;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tenantProvider"></param>
        public UserRepository(UserContext context, ITenantProvider tenantProvider) : base(context, tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }
    }
}