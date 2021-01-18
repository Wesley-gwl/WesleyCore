using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WesleyCore.User.Application.Queries.Login;
using WesleyCore.User.Application.Queries.Login.Dto;

namespace WesleyCore.User.Application.Queries.User
{
    /// <summary>
    /// 登录
    /// </summary>
    public class LoginHandler : IRequestHandler<LoginDto, UserDto>
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<UserDto> Handle(LoginDto request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}