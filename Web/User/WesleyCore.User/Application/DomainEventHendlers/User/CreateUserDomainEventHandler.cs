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
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(CreateUserDomainEvent input, CancellationToken cancellationToken)
        {
            var user = new Domain.User(input.UserName, input.PhoneNumber, input.Password, input.TenantId);
            await _userRepository.AddAsync(user);
            await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}