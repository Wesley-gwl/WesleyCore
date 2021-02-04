using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WesleyCore.User.Application.Queries.User.Dto;
using WesleyCore.User.Domain;
using WesleyCore.User.Proto;
using WesleyCore.Domin.Abstractions;
using Microsoft.EntityFrameworkCore;
using WesleyCore.User.Application.Queries.Login.Dto;
using AutoMapper;

namespace WesleyCore.User.Application.Queries.User
{
    /// <summary>
    /// 获取用户分页
    /// </summary>
    public class GerUserPagedHandler : IRequestHandler<GerUserPagedInput, PagedReturn<UserDto>>
    {
        /// <summary>
        /// 用户仓储
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// 实体映射
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="mapper"></param>
        public GerUserPagedHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 方法
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PagedReturn<UserDto>> Handle(GerUserPagedInput request, CancellationToken cancellationToken)
        {
            var query = _userRepository.GetAll();
            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                query = query.Where(p => p.PhoneNumber.Contains(request.SearchText.Trim()));
            }

            var count = await query.CountAsync(cancellationToken);
            var list = await query.OrderByDescending(p => p.CreateTime).AsNoTracking().PageBy(request).ToListAsync(cancellationToken);
            var userList = _mapper.Map<List<UserDto>>(list);
            return new PagedReturn<UserDto>(count, userList);
        }
    }
}