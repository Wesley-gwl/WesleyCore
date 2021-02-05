using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;
using WesleyCore.User.Domain;
using WesleyUntity;

namespace WesleyCore.User.Application.Commands.Member
{
    /// <summary>
    /// 创建会员
    /// </summary>
    public class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, bool>
    {
        /// <summary>
        ///
        /// </summary>
        private readonly IMemberRepository _memberRepository;

        private readonly IUserRepository _userRepository;

        private readonly IConfiguration _configuration;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="memberRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="configuration"></param>
        public CreateMemberCommandHandler(IMemberRepository memberRepository, IUserRepository userRepository, IConfiguration configuration)
        {
            _memberRepository = memberRepository;
            _configuration = configuration;
            _userRepository = userRepository;
        }

        /// <summary>
        /// 创建会员
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
        {
            //验证账号是否存在
            if (await _memberRepository.AnyAsync(p => p.PhoneNumber == request.PhoneNumber))
            {
                throw new WlException("号码已注册");
            }
            //验证手机号是否被添加
            if (await _userRepository.AnyAsync(p => p.PhoneNumber == request.PhoneNumber))
            {
                throw new WlException("此号码已被添加为员工,无法注册.请联系管理员");
            }
            var allowUserNumber = _configuration["Allocation:allowUserNumber"];
            var member = new Domain.Member(request.UserName, request.PhoneNumber, request.Company, allowUserNumber.ToInt(0).Value);
            member = await _memberRepository.InsertAndGetIdAsync(member);
            //优先创建member表数据,member.id可以获取到
            member.CreateMemberUser(request.Password);
            member.CreateMemberFeatureMenu();
            await _memberRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return true;
        }
    }
}