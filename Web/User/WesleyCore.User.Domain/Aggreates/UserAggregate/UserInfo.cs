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
        public bool IsAdmin { get; private set; }

        /// <summary>
        /// 状态
        /// </summary>
        public UserStatusEnum Status { get; private set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [StringLength(20)]
        public string IDCard { get; private set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        [StringLength(200)]
        public string ImageUrl { get; private set; }

        /// <summary>
        /// 地址
        /// </summary>
        [StringLength(200)]
        public string Address { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(200)]
        public string Memo { get; private set; }

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