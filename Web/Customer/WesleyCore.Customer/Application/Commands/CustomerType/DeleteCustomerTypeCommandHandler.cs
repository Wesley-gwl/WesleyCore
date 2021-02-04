using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WesleyCore.Customer.Application.Commands.CustomerType.Dto;
using WesleyCore.Customer.Domain.Repository;

namespace WesleyCore.Customer.Application.Commands.CustomerType
{
    /// <summary>
    /// 删除客户分类
    /// </summary>
    public class DeleteCustomerTypeCommandHandler : IRequestHandler<DeleteCustomerTypeCommand, bool>
    {
        /// <summary>
        /// 用户仓储
        /// </summary>
        private readonly ICustomerTypeRepository _customerTypeRepository;

        /// <summary>
        /// 构造
        /// </summary>
        public DeleteCustomerTypeCommandHandler(ICustomerTypeRepository customerTypeRepository)
        {
            _customerTypeRepository = customerTypeRepository;
        }

        /// <summary>
        /// 删除客户分类
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(DeleteCustomerTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _customerTypeRepository.GetAsync(request.Id);
            entity.VerifyUsed();
            await _customerTypeRepository.UnitOfWork.SaveEntitiesAsync();
            return await _customerTypeRepository.DeleteAsync(request.Id);
        }
    }
}