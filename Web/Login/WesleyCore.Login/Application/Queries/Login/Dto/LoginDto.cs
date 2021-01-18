using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WesleyCore.Application
{
    /// <summary>
    /// 登录
    /// </summary>
    public class LoginDto : IRequest<string>
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "账号不能为空。")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 租户id
        /// </summary>
        public long? TenantId { get; set; }

        /// <summary>
        /// ip地址
        /// </summary>
        public string IpAddress { get; set; }
    }
}