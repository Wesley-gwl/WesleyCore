using System;
using System.Collections.Generic;
using System.Text;
using WesleyCore.Domin.Abstractions;

namespace WesleyCore.Customer.Domain.Events.CustomerType
{
    /// <summary>
    /// 验证客户分类是否被使用
    /// </summary>
    public class VerifyCustomerTypeUsedDomainEvent : IDomainEvent
    {
        /// <summary>
        /// 构造
        /// </summary>
        public VerifyCustomerTypeUsedDomainEvent(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// 客户分类id
        /// </summary>
        public Guid Id { get; }
    }
}