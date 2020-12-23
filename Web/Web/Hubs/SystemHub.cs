using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WesleyCore.Web.Authorizer;

namespace WesleyPool.Web.Hubs
{
    /// <summary>
    /// SignalR集成线
    /// </summary>
    public class SystemHub : Hub
    {
        /// <summary>
        /// 线程池
        /// </summary>
        public static IDictionary<string, Guid> Pool = new ConcurrentDictionary<string, Guid>();

        /// <summary>
        /// 初始化连接
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            var cid = Context.ConnectionId;
            if (Context.GetHttpContext().Request.Cookies.TryGetValue("ck_token", out string token))
            {
                var auth = JWTUtil.DecodeToken(token);
                var uid = auth.UserId;
                Pool.TryAdd(cid, uid);
                return base.OnConnectedAsync();
            }
            throw new Exception("身份错误");
        }

        /// <summary>
        /// 断开
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            Pool.Remove(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}