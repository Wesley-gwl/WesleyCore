using AutoMapper;
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

        /// <summary>
        /// 会员仓储
        /// </summary>
        private readonly IMemberRepository _memberRepository;

        private readonly IMapper _mapper;

        /// <summary>
        /// 构造
        /// </summary>
        public LoginHandler(IUserRepository userRepository, IMemberRepository memberRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _memberRepository = memberRepository;
            _mapper = mapper;
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
            return _mapper.Map<UserDto>(user);
        }
    }
}