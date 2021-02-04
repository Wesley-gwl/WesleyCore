using AutoMapper;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WesleyCore.User.Application.Queries.User.Dto;
using WesleyCore.User.Proto;

namespace WesleyCore.User.GrpcService
{
    /// <summary>
    /// 用户聚合服务
    /// </summary>
    [Authorize("permission")]//token验证
    public class UserService : IUserService.IUserServiceBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserService> _logger;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="logger"></param>
        public UserService(IMediator mediator, ILogger<UserService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<UserPagedOutput> GetUserList(GetUserPagedForm input, ServerCallContext context)
        {
            _logger.LogInformation($"开始grpc=》获取用户列表服务，{DateTime.Now}");
            var paged = await _mediator.Send(new GerUserPagedInput()
            {
                SearchText = input.SearchText,
                Page = input.Paged,
                Rows = input.Rows
            }, context.CancellationToken);
            var re = new UserPagedOutput() { Total = paged.Total };
            paged.Rows.ForEach(item =>
            {
                re.UserItem.Add(new UserItemDto()
                {
                    PhoneNumber = item.PhoneNumber,
                    UserName = item.UserName
                });
            });
            _logger.LogInformation($"结束grpc=》获取用户列表服务，{DateTime.Now}");
            return re;
        }
    }
}