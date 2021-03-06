﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WesleyCore.Enums;

namespace WesleyCore.Customer.Application.Commands.CustomerType.Dto
{
    /// <summary>
    /// 创建修改客户分类命令
    /// </summary>
    public class CreateOrEditCustomerTypeCommand : IRequest
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="memo"></param>
        public CreateOrEditCustomerTypeCommand(Guid? id, CustomerTypeEnum type, string name, string memo)
        {
            Id = id;
            Type = type;
            Name = name;
            Memo = memo;
        }

        /// <summary>
        /// 主键
        /// </summary>
        public Guid? Id { get; private set; }

        /// <summary>
        /// 分类
        /// </summary>
        public CustomerTypeEnum Type { get; private set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; private set; }
    }
}