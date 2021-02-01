using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace WesleyCore.Customer.Domain.Events
{
    /// <summary>
    /// 创建客户分类
    /// </summary>
    public class CreateCustomerTypeDomainEvent : INotification
    {
        public CustomerType CustomerType { get; }

        public CreateCustomerTypeDomainEvent(CustomerType customerType)
        {
            CustomerType = customerType;
        }
    }
}