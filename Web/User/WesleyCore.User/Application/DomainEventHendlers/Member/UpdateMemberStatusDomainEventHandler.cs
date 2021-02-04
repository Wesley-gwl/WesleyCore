using System.Threading;
using System.Threading.Tasks;
using WesleyCore.Domin.Abstractions;
using WesleyCore.User.Domain;

namespace WesleyCore.User.Application.DomainEvents.Member.UpdateMemberStatus
{
    /// <summary>
    /// 更新会员状态领域事件
    /// </summary>
    public class UpdateMemberStatusDomainEventHandler : IDomainEventHandler<UpdateMemberStatusDomainEvent>
    {
        private readonly IMemberRepository _memberRepository;

        /// <summary>
        /// 构造
        /// </summary>
        public UpdateMemberStatusDomainEventHandler(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        /// <summary>
        /// 更新会员状态
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(UpdateMemberStatusDomainEvent request, CancellationToken cancellationToken)
        {
            await _memberRepository.UpdateAsync(request.Member, cancellationToken);
        }
    }
}