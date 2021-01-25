using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static WesleyCore.User.Proto.ILoginService;

namespace IdentityServer.GrpcService
{
    /// <summary>
    /// grpc服务发现帮助类
    /// </summary>
    public interface IGrpcServiceHelper
    {
        /// <summary>
        /// 获取登录服务
        /// </summary>
        /// <returns></returns>
        Task<ILoginServiceClient> GetLoginService();
    }
}