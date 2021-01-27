using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WesleyCore.User.Application.Queries.Login;
using WesleyCore.User.Application.Queries.Login.Dto;
using WesleyCore.User.Domain;
using WesleyCore.User.Domain.Const;
using WesleyCore.User.Domain.Enums.User;
using WesleyUntity;

namespace WesleyCore.User.Application.Queries.User
{
    /// <summary>
    /// 登录
    /// </summary>
    public class LoginHandler : IRequestHandler<LoginInput, UserDto>
    {
        /// <summary>
        /// 用户仓储
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// 会员仓储
        /// </summary>
        private readonly IMemberRepository _memberRepository;

        /// <summary>
        /// automap
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger<LoginHandler> _logger;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="memberRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public LoginHandler(IUserRepository userRepository, IMemberRepository memberRepository, IMapper mapper, ILogger<LoginHandler> logger)
        {
            _userRepository = userRepository;
            _memberRepository = memberRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<UserDto> Handle(LoginInput request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{request.PhoneNumber}尝试登录");
            var user = await _userRepository.FirstOrDefaultAsync(p => p.PhoneNumber == request.PhoneNumber);
            if (user == null)
            {
                throw new WlException("账号不存在");
            }
            user.VerifyLogin(request.Password);

            var member = await _memberRepository.GetMemberShipInclude(user.TenantId);
            member.VerifyMemberShip();
            return _mapper.Map<UserDto>(user);
        }
    }
}