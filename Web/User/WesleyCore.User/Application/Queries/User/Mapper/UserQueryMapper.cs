using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WesleyCore.User.Application.Queries.Login.Dto;
using WesleyCore.User.Proto;

namespace WesleyCore.User.Application.Queries.User.Mapper
{
    /// <summary>
    /// 用户类映射
    /// </summary>
    public class UserQueryMapper : Profile
    {
        /// <summary>
        /// 构造
        /// </summary>
        public UserQueryMapper()
        {
            CreateMap<Domain.User, UserDto>();
        }
    }
}