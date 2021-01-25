using ConsulRegister;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
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
    public class GrpcServiceHelper : IGrpcServiceHelper
    {
        /// <summary>
        /// 获取登录服务
        /// </summary>
        public async Task<IUserServiceClient> GetUserService()
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);//允许使用不加密的HTTP/2协议
            var address = await GrpcServiceConsulExtension.GetGrpcServiceHttps(GrpcServiceCoust.UserService);
            var channel = GrpcChannel.ForAddress(address);
            return new IUserServiceClient(channel);
        }
    }
}