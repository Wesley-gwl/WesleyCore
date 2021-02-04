using MediatR;
using System.ComponentModel.DataAnnotations;

namespace WesleyCore.User.Application.Commands.Member
{
    /// <summary>
    /// 新建会员
    /// </summary>
    public class CreateMemberCommand : IRequest<bool>
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "账号不能为空")]
        [RegularExpression(@"^1[3456789]\d{9}$", ErrorMessage = "请输入正确的手机号码")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "密码不能为空")]
        public string Password { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "用户名不能为空")]
        public string UserName { get; set; }

        /// <summary>
        /// 公司
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "公司不能为空")]
        public string Company { get; set; }
    }
}