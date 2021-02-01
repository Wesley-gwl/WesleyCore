using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WesleyCore.Customer.Domain.Repository;

namespace WesleyCore.Customer.Application.Commands
{
    /// <summary>
    /// 创建修改客户分类命令
    /// </summary>
    public class CreateOrEditCustomerTypeCommandHandler : IRequestHandler<CreateOrEditCustomerTypeCommand>
    {
        /// <summary>
        /// 用户仓储
        /// </summary>
        private readonly ICustomerTypeRepository _customerTypeRepository;

        /// <summary>
        /// 实体映射
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="customerTypeRepository"></param>
        /// <param name="mapper"></param>
        public CreateOrEditCustomerTypeCommandHandler(ICustomerTypeRepository customerTypeRepository, IMapper mapper)
        {
            _customerTypeRepository = customerTypeRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 创新修改实体
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Unit> Handle(CreateOrEditCustomerTypeCommand request, CancellationToken cancellationToken)
        {
            if (request.Id.HasValue)
            {
                await Update(request);
            }
            else
            {
                await Create(request);
            }
            return new Unit();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task Create(CreateOrEditCustomerTypeCommand input)
        {
            var model = new Domain.CustomerType(input.Type, input.Name, input.Memo);
            await _customerTypeRepository.AddAsync(model);
            await _customerTypeRepository.UnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task Update(CreateOrEditCustomerTypeCommand input)
        {
            var model = await _customerTypeRepository.GetAsync(input.Id.Value);
            model = _mapper.Map<Domain.CustomerType>(input);
            await _customerTypeRepository.UpdateAsync(model);
        }
    }
}