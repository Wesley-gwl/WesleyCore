using System;
using System.Collections.Generic;
using System.Text;
using WesleyCore.Domin.Abstractions;

namespace WesleyCore.Domain.Events.Member
{
    /// <summary>
    /// 获取最新的有效时间
    /// </summary>
    public class GetMemberShipDomainEvent : IDomainEvent
    {
        public int TenantId { get; set; }

        public GetMemberShipDomainEvent(int tenantId)
        {
            this.TenantId = tenantId;
        }
    }
}