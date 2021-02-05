using System;
using System.Collections.Generic;
using System.Text;
using WesleyCore.Domin.Abstractions;

namespace WesleyCore.User.Domain.Events.Feature
{
    /// <summary>
    /// 创建租户会员菜单
    /// </summary>
    public class CreateFeatureMenuDomainEvent : IDomainEvent
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="id"></param>
        public CreateFeatureMenuDomainEvent(int id)
        {
            TenantId = id;
        }

        /// <summary>
        /// 租户id
        /// </summary>
        public int TenantId { get; }
    }
}