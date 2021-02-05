using System;
using System.Collections.Generic;
using System.Text;
using WesleyCore.Infrastruction.Core;
using WesleyCore.Infrastructure;
using WesleyCore.User.Domain;
using WesleyCore.User.Domain.Repository;

namespace WesleyCore.User.Infrastructure.Repository
{
    /// <summary>
    /// 角色仓储
    /// </summary>
    public class RoleRepository : Repository<Role, Guid, UserContext>, IRoleRepository
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
        public RoleRepository(UserContext context, ITenantProvider tenantProvider) : base(context, tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }
    }
}