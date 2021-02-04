using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WesleyCore.Customer.Domain.Events.CustomerType;
using WesleyCore.Customer.Domain.Repository;
using WesleyCore.Domin.Abstractions;

namespace WesleyCore.Customer.Application.DomainEventHendlers.Customer
{
    /// <summary>
    ///
    /// </summary>
    public class VerifyCustomerTypeUsedDomainEventHendler : IDomainEventHandler<VerifyCustomerTypeUsedDomainEvent>
    {
        /// <summary>
        /// 客户仓储
        /// </summary>
        private readonly ICustomerRepository _customerRepository;

        /// <summary>
        /// 构造
        /// </summary>
        public VerifyCustomerTypeUsedDomainEventHendler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// 验证客户分类是否被使用
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(VerifyCustomerTypeUsedDomainEvent notification, CancellationToken cancellationToken)
        {
            if (await _customerRepository.GetAll().AnyAsync(p => p.CustomerTypeId == notification.Id))
            {
                throw new WlException("客户分类已被使用,无法删除");
            }
        }
    }
}