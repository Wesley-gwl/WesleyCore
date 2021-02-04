using System;
using System.Collections.Generic;
using System.Text;
using WesleyCore.Infrastruction.Core;

namespace WesleyCore.Customer.Domain.Repository
{
    public interface ICustomerRepository : IRepository<Customer, Guid>
    {
    }
}