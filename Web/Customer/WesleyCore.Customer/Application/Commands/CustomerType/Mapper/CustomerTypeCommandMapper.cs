using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WesleyCore.Customer.Application.Commands.CustomerType.Dto;

namespace WesleyCore.Customer.Application.Commands
{
    /// <summary>
    /// 映射
    /// </summary>
    public class CustomerTypeCommandMapper : Profile
    {
        /// <summary>
        /// 构造
        /// </summary>
        public CustomerTypeCommandMapper()
        {
            CreateMap<CreateOrEditCustomerTypeCommand, Domain.CustomerType>();
        }
    }
}