using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WesleyCore.Domin.Abstractions;
using WesleyCore.User.Domain.Enums.User;

namespace WesleyCore.User
{
    /// <summary>
    /// 值类型
    /// </summary>
    [Owned]
    public class UserInfo : ValueObject
    {
        /// <summary>
        /// 是否是管理员
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public UserStatusEnum Status { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [StringLength(20)]
        public string IDCard { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        [StringLength(200)]
        public string ImageUrl { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [StringLength(200)]
        public string Address { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Memo { get; set; }

        /// <summary>
        /// 原子性
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            // Using a yield return statement to return each element one at a time
            yield return IsAdmin;
            yield return Status;
            yield return IDCard;
            yield return ImageUrl;
            yield return Address;
            yield return Memo;
        }
    }
}