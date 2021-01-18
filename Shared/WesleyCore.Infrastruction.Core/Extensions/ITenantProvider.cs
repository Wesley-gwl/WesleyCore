using System;
using System.Collections.Generic;
using System.Text;

namespace WesleyCore.Infrastructure
{
    public interface ITenantProvider
    {
        /// <summary>
        /// 获取租户id
        /// </summary>
        /// <returns></returns>
        int GetTenantId();
    }
}