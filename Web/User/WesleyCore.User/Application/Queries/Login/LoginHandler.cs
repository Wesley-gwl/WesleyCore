using MediatR;
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
    public class LoginHandler : IRequestHandler<LoginDto, UserDto>
    {
        /// <summary>
        /// 用户仓储
        /// </summary>
        private readonly IUserRepository _userRepository;

        private readonly IMemberRepository _memberRepository;

        /// <summary>
        /// 构造
        /// </summary>
        public LoginHandler(IUserRepository userRepository, IMemberRepository memberRepository)
        {
            _userRepository = userRepository;
            _memberRepository = memberRepository;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<UserDto> Handle(LoginDto request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FirstOrDefaultAsync(p => p.PhoneNumber == request.PhoneNumber);
            if (user == null)
            {
                throw new Exception("账号不存在");
            }
            user.VerifyLogin(request.Password);

            var member = await _memberRepository.GetMemberShipInclude(user.TenantId);
            member.VerifyMemberShip();
            return new UserDto()
            {
                IsAdmin = user.UserInfo.IsAdmin,
                PhoneNumber = user.PhoneNumber,
                TenantId = user.TenantId,
                UserId = user.Id,
                UserName = user.UserName
            };
        }
    }
}