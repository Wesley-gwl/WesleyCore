using System;
using System.Collections.Generic;
using System.Text;
using WesleyCore.Customer.Domain.Repository;
using WesleyCore.Infrastruction.Core;
using WesleyCore.Infrastructure;

namespace WesleyCore.Customer.Infrastructure.Repository
{
    public class CustomerTypeRepository : Repository<Domain.CustomerType, Guid, CustomerContext>, ICustomerTypeRepository
    {
        /// <summary>
        ///
        /// </summary>
        private readonly CustomerContext _context;

        /// <summary>
        /// 租户查询
        /// </summary>
        private readonly ITenantProvider _tenantProvider;

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <param name="tenantProvider"></param>
        public CustomerTypeRepository(CustomerContext context, ITenantProvider tenantProvider) : base(context, tenantProvider)
        {
            _context = context;
            _tenantProvider = tenantProvider;
        }
    }
}