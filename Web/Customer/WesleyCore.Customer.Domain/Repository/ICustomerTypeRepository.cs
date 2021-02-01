using System;
using System.Collections.Generic;
using System.Text;
using WesleyCore.Infrastruction.Core;

namespace WesleyCore.Customer.Domain.Repository
{
    public interface ICustomerTypeRepository : IRepository<CustomerType, Guid>
    {
    }
}