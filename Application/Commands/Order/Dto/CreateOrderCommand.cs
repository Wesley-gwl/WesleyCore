using MediatR;

namespace WesleyCore.Application.Commands
{
    /// <summary>
    /// 创建订单
    /// </summary>
    public class CreateOrderCommand : IRequest<long>
    {
        //ublic CreateOrderCommand() { }
        //public CreateOrderCommand(int itemCount)
        //{
        //    ItemCount = itemCount;
        //}
        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string UserName { get; set; }

        public string Street { get; private set; }

        /// <summary>
        /// 成是
        /// </summary>
        public string City { get; private set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string ZipCode { get; private set; }
    }
}