using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WesleyCore.User.Proto;
using static WesleyCore.User.Proto.IUserService;

namespace Wesley.GrpcService
{
    /// <summary>
    /// grpc服务发现帮助类
    /// </summary>
    public interface IGrpcServiceHelper
    {
        /// <summary>
        /// 获取用户服务
        /// </summary>
        /// <returns></returns>
        Task<IUserServiceClient> GetUserService();
    }
}