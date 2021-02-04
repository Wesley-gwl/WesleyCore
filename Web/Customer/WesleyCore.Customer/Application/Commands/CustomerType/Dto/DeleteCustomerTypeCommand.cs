using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WesleyCore.Customer.Application.Commands.CustomerType.Dto
{
    /// <summary>
    /// 删除客户分类
    /// </summary>
    public class DeleteCustomerTypeCommand : IRequest<bool>
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="id"></param>
        public DeleteCustomerTypeCommand(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// 客户分类id
        /// </summary>
        public Guid Id { get; set; }
    }
}