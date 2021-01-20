using WesleyCore.Domin.Abstractions;

namespace WesleyCore.User.Domain
{
    /// <summary>
    /// 更新会员状态领域事件
    /// </summary>
    public class UpdateMemberStatusDomainEvent : IDomainEvent
    {
        /// <summary>
        /// 构造
        /// </summary>
        public Member Member { get; }

        /// <summary>
        /// 更新会员状态
        /// </summary>
        /// <param name="member"></param>
        public UpdateMemberStatusDomainEvent(Member member)
        {
            Member = member;
        }
    }
}