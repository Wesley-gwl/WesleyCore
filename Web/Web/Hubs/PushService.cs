using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WesleyPool.Web.Hubs
{
    /// <summary>
    /// 推送服务
    /// </summary>
    public class PushService : BackgroundService
    {
        private readonly IHubContext<SystemHub> _hubContext;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="HubContext"></param>
        public PushService(IHubContext<SystemHub> HubContext)
        {
            _hubContext = HubContext;
        }

        /// <summary>
        /// 后台轮询
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var pool = SystemHub.Pool.ToLookup(v => v.Value);
                await Task.Delay(5 * 1000, stoppingToken);
            }
        }
    }
}