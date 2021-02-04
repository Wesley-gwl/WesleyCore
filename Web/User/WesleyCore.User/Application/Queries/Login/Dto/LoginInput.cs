using MediatR;
using WesleyCore.User.Application.Queries.Login.Dto;

namespace WesleyCore.User.Application.Queries.Login
{
    /// <summary>
    /// 登入dto
    /// </summary>
    public class LoginInput : IRequest<UserDto>
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// ip地址
        /// </summary>
        public string IpAddress { get; set; }
    }
}