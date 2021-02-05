using System;
using System.Collections.Generic;
using System.Text;
using WesleyCore.Infrastruction.Core;

namespace WesleyCore.User.Domain.Repository
{
    /// <summary>
    /// 角色仓储
    /// </summary>
    public interface IRoleRepository : IRepository<Role, Guid>
    {
    }
}