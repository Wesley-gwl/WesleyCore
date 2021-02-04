using System.Threading;
using System.Threading.Tasks;
using WesleyCore.Domin.Abstractions;
using WesleyCore.User.Domain;
using WesleyCore.User.Domain.Events.User;

namespace WesleyCore.User.Application.DomainEvents.User
{
    /// <summary>
    /// 创建用户
    /// </summary>
    public class CreateUserDomainEventHandler : IDomainEventHandler<CreateUserDomainEvent>
    {
        /// <summary>
        /// 用户仓储
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// 构造
        /// </summary>
        public CreateUserDomainEventHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(CreateUserDomainEvent @event, CancellationToken cancellationToken)
        {
            var user = new Domain.User(@event.UserName, @event.PhoneNumber, @event.Password, @event.TenantId);
            await _userRepository.AddAsync(user);
            if (@event.IsCreateMemu)
            {
                //初始化菜单
                //todo
            }
            //发送消息
            //todo
            await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}